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
                vqts.Add(new VQT(GetValueAtTime(timePeriod.StartTime), timePeriod.StartTime, new Quality()));
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
                Quality quality;
                if (startTime.Ticks % TimeSpan.TicksPerSecond == 0)
                {
                    // we happen to be at a whole second, mark this as Good/Raw
                    quality = new Quality();
                }
                else
                {
                    // not at a whole second, mark this as Good/Interpolated
                    quality = new Quality() { HDAQuality = HDAQuality.Interpolated };
                }

                // add generated value
                vqts.Add(new VQT(GetValueAtTime(startTime), startTime, quality));

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
            DateTime beforeTime = startTime.AddTicks(-(startTime.Ticks % TimeSpan.TicksPerSecond));
            object beforeValue = GetValueAtTime(beforeTime);
            object valueAtStart;
            if (sampleAndHold || !(beforeValue is float || beforeValue is double))
            {
                // for sample and hold, just use (hold) the before value
                valueAtStart = beforeValue;
            }
            else
            {
                DateTime afterTime = beforeTime.AddSeconds(1);
                double afterValue = Convert.ToDouble(GetValueAtTime(afterTime));
                double beforeAsDouble = Convert.ToDouble(beforeValue);

                // calculate value at start based on value/time ratio
                double start = beforeAsDouble + (afterTime - startTime).Milliseconds * (afterValue - beforeAsDouble) / 1000;
                if (beforeValue is float)
                {
                    valueAtStart = Convert.ToSingle(start);
                }
                else
                {
                    valueAtStart = start;
                }
            }

            return new VQT(valueAtStart, startTime, new Quality() { HDAQuality = HDAQuality.Interpolated });
        }

        public VQT GetValueAt(DateTime time)
        {
            // align value to second boundary (assumes sample and hold)
            DateTime alignedTime = time.AddTicks(-(time.Ticks % TimeSpan.TicksPerSecond));
            return new VQT(GetValueAtTime(alignedTime), time, new Quality());
        }

        protected abstract object GetValueAtTime(DateTime time);
    }
}
