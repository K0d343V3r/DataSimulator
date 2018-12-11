using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Helpers
{
    public abstract class DataGenerator
    {
        public IList<VQT> GetValues(TimePeriod timePeriod, InitialValue initialValue, int maxCount)
        {
            var vqts = new List<VQT>();

            // get projected value count
            int projectedCount = CalculateProjectedCount(timePeriod, initialValue);

            // add initial value
            if (timePeriod.StartTime.Ticks % TimeSpan.TicksPerSecond == 0)
            {
                // start time is at whole second, generate Good/Raw value at this time
                var value = GetValueAtTime(timePeriod.StartTime);
                Quality quality = new Quality()
                {
                    Major = value == null ? MajorQuality.Bad : MajorQuality.Good
                };
                vqts.Add(new VQT(value, timePeriod.StartTime, quality));
            }
            else if (initialValue != InitialValue.None || (maxCount > 0 && projectedCount > maxCount))
            {
                // initial value requested or we need to interpolate (requires value at initial time)
                vqts.Add(InterpolateValueAt(timePeriod.StartTime, initialValue == InitialValue.SampleAndHold));
            }

            // add subsequent values
            DateTime startTime;
            long stepTicks;
            if (maxCount == 0 || projectedCount <= maxCount)
            {
                // we need to generate raw values, start at next whole second
                long fractionTicks = timePeriod.StartTime.Ticks % TimeSpan.TicksPerSecond;
                startTime = timePeriod.StartTime.AddTicks(TimeSpan.TicksPerSecond - fractionTicks);

                // and generate one value per second
                stepTicks = TimeSpan.TicksPerSecond;
            }
            else
            {
                // we need to generate interpolated values, divide time span to generate maxCount values
                stepTicks = (timePeriod.EndTime - timePeriod.StartTime).Ticks / maxCount;

                // skip value at start if we already have one
                startTime = vqts.Count > 0 ? timePeriod.StartTime.AddTicks(stepTicks) : timePeriod.StartTime;
            }

            while (startTime < timePeriod.EndTime)
            {
                var value = GetValueAtTime(startTime);
                if (maxCount == 0 || projectedCount <= maxCount)
                {
                    // not interpolating, use raw value
                    Quality quality = new Quality()
                    {
                        Major = value == null ? MajorQuality.Bad : MajorQuality.Good
                    };
                    vqts.Add(new VQT(value, startTime, quality));
                }
                else if (value != null)
                {
                    // interpolating and we got a good value, use it
                    Quality quality = new Quality()
                    {
                        // if value at second boundary, mark it as a raw value
                        HDAQuality = startTime.Ticks % TimeSpan.TicksPerSecond == 0 ? HDAQuality.Raw : HDAQuality.Interpolated
                    };
                    vqts.Add(new VQT(value, startTime, quality));
                }
                else
                {
                    // interpolating and we got a bad value, interpolate non-bad
                    vqts.Add(InterpolateValueAt(startTime, initialValue == InitialValue.SampleAndHold));
                }

                // advance to next time boundary
                startTime = startTime.AddTicks(stepTicks);
            }

            return vqts;
        }

        private int CalculateProjectedCount(TimePeriod timePeriod, InitialValue initialValue)
        {
            TimeSpan span = timePeriod.EndTime - timePeriod.StartTime;
            int count = (int)(span.Ticks / TimeSpan.TicksPerSecond);
            if (timePeriod.StartTime.Ticks % TimeSpan.TicksPerSecond > 0 && initialValue != InitialValue.None)
            {
                // start is at fraction of a second, add one if initial value requested
                count++;
            }

            return count;
        }

        //
        // See OPC HDA 1.2 Specification, section 2.9.2.4.
        // Reference: https://www.vegvesen.no/_attachment/1467098/binary/1124390
        //
        private VQT InterpolateValueAt(DateTime time, bool sampleAndHold)
        {
            var quality = new Quality()
            {
                HDAQuality = HDAQuality.Interpolated
            };

            VQT beforeValue = FindNearestGoodValue(time, false);

            if (sampleAndHold || !(beforeValue.Value is float || beforeValue.Value is double))
            {
                // for sample and hold, just use (hold) the before value
                quality.Major = beforeValue.Quality.Major;
                return new VQT(beforeValue.Value, time, quality);
            }
            else
            {
                VQT afterValue = FindNearestGoodValue(time, true);

                double afterAsDouble = Convert.ToDouble(afterValue.Value);
                double beforeAsDouble = Convert.ToDouble(beforeValue.Value);

                // for linear, interpolate based on time/value ratio
                long timeDifference = (time - beforeValue.Time).Ticks;
                double valueDifference = afterAsDouble - beforeAsDouble;
                double value = beforeAsDouble + timeDifference * valueDifference / TimeSpan.TicksPerSecond;

                if (beforeValue.Quality.Major == MajorQuality.Uncertain || 
                    afterValue.Quality.Major == MajorQuality.Uncertain)
                {
                    quality.Major = MajorQuality.Uncertain;
                }

                if (beforeValue.Value is float)
                {
                    return new VQT(Convert.ToSingle(value), time, quality);
                }
                else
                {
                    return new VQT(value, time, quality);
                }
            }
        }

        private VQT FindNearestGoodValue(DateTime time, bool forwards)
        {
            long fractionTicks = time.Ticks % TimeSpan.TicksPerSecond;
            DateTime startTime = time.AddTicks(forwards ? TimeSpan.TicksPerSecond - fractionTicks : -fractionTicks);

            bool skippedNonGood = false;
            object value;
            do
            {
                value = GetValueAtTime(startTime);
                if (value == null)
                {
                    skippedNonGood = true;
                    startTime = startTime.AddTicks(forwards ? TimeSpan.TicksPerSecond : -TimeSpan.TicksPerSecond);
                }
            }
            while (value == null);

            var quality = new Quality()
            {
                Major = skippedNonGood ? MajorQuality.Uncertain : MajorQuality.Good
            };

            return new VQT(value, startTime, quality);
        }

        public VQT GetValueAt(DateTime time)
        {
            // align value to second boundary (assumes sample and hold)
            DateTime alignedTime = time.AddTicks(-(time.Ticks % TimeSpan.TicksPerSecond));
            var value = GetValueAtTime(alignedTime);

            var quality = new Quality()
            {
                Major = value == null ? MajorQuality.Bad : MajorQuality.Good
            };

            return new VQT(value, time, quality);
        }

        protected abstract object GetValueAtTime(DateTime time);
    }
}
