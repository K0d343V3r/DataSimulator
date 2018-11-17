using DataServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Helpers
{
    public abstract class DataGenerator
    {
        public IList<VQT> GetValues(TimePeriod timePeriod, InitialValue initialValue)
        {
            List<VQT> vtqs = new List<VQT>();

            // add value at start
            if (timePeriod.StartTime.Millisecond == 0)
            {
                // start at whole second, generate raw value at start
                vtqs.Add(new VQT(GetValueAtTime(timePeriod.StartTime), timePeriod.StartTime, new Quality()));
            }
            else if (initialValue != InitialValue.None)
            {
                // value at start requested, interpolate one
                vtqs.Add(CreateInterpolatedValue(timePeriod.StartTime, initialValue == InitialValue.SampleAndHold));
            }

            // next value should be at the next full second
            DateTime startTime = timePeriod.StartTime.AddMilliseconds(1000 - timePeriod.StartTime.Millisecond);
            while (startTime <= timePeriod.EndTime)
            {
                // all generated values are Good/Raw
                vtqs.Add(new VQT(GetValueAtTime(startTime), startTime, new Quality()));

                // generate a value every second
                startTime.AddSeconds(1);
            }

            return vtqs;
        }

        private VQT CreateInterpolatedValue(DateTime startTime, bool sampleAndHold)
        {
            DateTime beforeTime = startTime.AddMilliseconds(-startTime.Millisecond);
            float beforeValue = (float)GetValueAtTime(beforeTime);
            float valueAtStart;
            if (sampleAndHold)
            {
                // for sample and hold, just use (hold) the before value
                valueAtStart = beforeValue;
            }
            else
            {
                DateTime afterTime = startTime.AddMilliseconds(1000 - startTime.Millisecond);
                float afterValue = (float)GetValueAtTime(afterTime);

                // calculate value at start based on value/time ratio
                valueAtStart = (startTime - beforeTime).Milliseconds * (afterValue - beforeValue) / 1000;
            }

            return new VQT(valueAtStart, startTime, new Quality() { HDAQuality = HDAQuality.Interpolated });
        }

        public VQT GetValueAt(DateTime time)
        {
            // align value to second boundary (assumes sample and hold)
            DateTime alignedTime = time.AddMilliseconds(-time.Millisecond);
            return new VQT(GetValueAtTime(alignedTime), time, new Quality());
        }

        protected abstract object GetValueAtTime(DateTime time);
    }
}
