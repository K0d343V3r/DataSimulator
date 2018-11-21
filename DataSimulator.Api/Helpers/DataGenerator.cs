using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Helpers
{
    public abstract class DataGenerator
    {
        public IList<VQT> GetValues(TimePeriod timePeriod, InitialValue initialValue)
        {
            List<VQT> vqts = new List<VQT>();

            // add value at start
            if (timePeriod.StartTime.Ticks % TimeSpan.TicksPerSecond == 0)
            {
                // start is at whole second, generate raw value at start
                vqts.Add(new VQT(GetValueAtTime(timePeriod.StartTime), timePeriod.StartTime, new Quality()));
            }
            else if (initialValue == InitialValue.None)
            {
                // create bad quality data point at start
                Quality quality = new Quality() { Major = MajorQuality.Bad, HDAQuality = HDAQuality.NoData };
                vqts.Add(new VQT(null, timePeriod.StartTime, quality));
            }
            else
            {
                // value at start requested, interpolate one
                vqts.Add(CreateInterpolatedValue(timePeriod.StartTime, initialValue == InitialValue.SampleAndHold));
            }

            // next value should be at the next full second (remove milliseconds)
            DateTime nextTime = timePeriod.StartTime.AddTicks(-(timePeriod.StartTime.Ticks % TimeSpan.TicksPerSecond));
            nextTime = nextTime.AddSeconds(1);
            while (nextTime < timePeriod.EndTime)
            {
                // all generated values are Good/Raw
                vqts.Add(new VQT(GetValueAtTime(nextTime), nextTime, new Quality()));

                // generate a value every second
                nextTime = nextTime.AddSeconds(1);
            }

            return vqts;
        }

        private VQT CreateInterpolatedValue(DateTime startTime, bool sampleAndHold)
        {
            DateTime beforeTime = startTime.AddTicks(-(startTime.Ticks % TimeSpan.TicksPerSecond));
            float beforeValue = (float)GetValueAtTime(beforeTime);
            float valueAtStart;
            if (sampleAndHold)
            {
                // for sample and hold, just use (hold) the before value
                valueAtStart = beforeValue;
            }
            else
            {
                DateTime afterTime = beforeTime.AddSeconds(1);
                float afterValue = (float)GetValueAtTime(afterTime);

                // calculate value at start based on value/time ratio
                valueAtStart = beforeValue + (afterTime - startTime).Milliseconds * (afterValue - beforeValue) / 1000;
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
