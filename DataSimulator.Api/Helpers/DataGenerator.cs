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
                Quality quality = new Quality() { Major = value == null ? MajorQuality.Bad : MajorQuality.Good };
                vqts.Add(new VQT(value, timePeriod.StartTime, quality));
            }
            else if (initialValue != InitialValue.None || (maxCount > 0 && projectedCount > maxCount))
            {
                // initial value requested, or we have too many items, which require a value at start anyway
                vqts.Add(CreateInterpolatedValue(timePeriod.StartTime, initialValue == InitialValue.SampleAndHold));
            }

            // add subsequent values
            DateTime startTime;
            long stepTicks;
            if (maxCount == 0 || projectedCount <= maxCount)
            {
                long fractionTicks = timePeriod.StartTime.Ticks % TimeSpan.TicksPerSecond;

                // we start at first whole second
                startTime = timePeriod.StartTime.AddTicks(TimeSpan.TicksPerSecond - fractionTicks);

                // and generate one value per second
                stepTicks = TimeSpan.TicksPerSecond;
            }
            else
            {
                // generate exactly maxCount values
                stepTicks = (timePeriod.EndTime - timePeriod.StartTime).Ticks / maxCount;

                // skip value at start if we already have one
                startTime = vqts.Count > 0 ? timePeriod.StartTime.AddTicks(stepTicks) : timePeriod.StartTime;
            }

            while (startTime < timePeriod.EndTime)
            {
                var value = GetValueAtTime(startTime);
                Quality quality = new Quality() { Major = value == null ? MajorQuality.Bad : MajorQuality.Good };
                if (startTime.Ticks % TimeSpan.TicksPerSecond > 0)
                {
                    // value is not at a second boundary, mark this as Interpolated
                    quality.HDAQuality = HDAQuality.Interpolated;
                }

                // add generated value
                vqts.Add(new VQT(value, startTime, quality));

                // and advance to next time
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

        private VQT CreateInterpolatedValue(DateTime startTime, bool sampleAndHold)
        {
            object value = null;
            DateTime beforeTime = startTime.AddTicks(-(startTime.Ticks % TimeSpan.TicksPerSecond));
            object beforeValue = GetValueAtTime(beforeTime);
            if (beforeValue != null)
            {
                if (sampleAndHold || !(beforeValue is float || beforeValue is double))
                {
                    // for sample and hold, just use (hold) the before value
                    value = beforeValue;
                }
                else
                {
                    DateTime afterTime = beforeTime.AddSeconds(1);
                    object afterValue = GetValueAtTime(afterTime);
                    if (afterValue != null)
                    {
                        double afterAsDouble = Convert.ToDouble(afterValue);
                        double beforeAsDouble = Convert.ToDouble(beforeValue);

                        // calculate value at start based on value/time ratio
                        double start = beforeAsDouble + (afterTime - startTime).Milliseconds * (afterAsDouble - beforeAsDouble) / 1000;
                        if (beforeValue is float)
                        {
                            value = Convert.ToSingle(start);
                        }
                        else
                        {
                            value = start;
                        }
                    }
                }
            }

            var quality = new Quality()
            {
                Major = value == null ? MajorQuality.Bad : MajorQuality.Good,
                HDAQuality = HDAQuality.Interpolated
            };

            return new VQT(value, startTime, quality);
        }

        public VQT GetValueAt(DateTime time)
        {
            // align value to second boundary (assumes sample and hold)
            DateTime alignedTime = time.AddTicks(-(time.Ticks % TimeSpan.TicksPerSecond));
            var value = GetValueAtTime(alignedTime);
            var quality = new Quality() { Major = value == null ? MajorQuality.Bad : MajorQuality.Good };
            return new VQT(value, time, quality);
        }

        protected abstract object GetValueAtTime(DateTime time);
    }
}
