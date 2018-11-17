using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServices.Models;

namespace DataServices.Helpers
{
    public static class DataAggregator
    {
        public static IList<VQT> InterpolateValues(IList<VQT> rawValues, TimePeriod timePeriod, int count)
        {
            if (rawValues.Count == 0 || count == 0 || timePeriod.TimeSpan.Ticks == 0)
            {
                return new List<VQT>();
            }
            else
            {
                List<VQT> values = new List<VQT>(count);
                long stepTicks = (timePeriod.EndTime - timePeriod.StartTime).Ticks / count;
                int rawIndex = 0;
                DateTime fencePost = timePeriod.StartTime;
                while (fencePost <= timePeriod.EndTime)
                {
                    while (rawIndex < rawValues.Count && rawValues[rawIndex].Time < fencePost)
                    {
                        rawIndex++;
                    }
                    if (rawIndex == rawValues.Count || rawValues[rawIndex].Time > fencePost)
                    {
                        // ran out of values (less raw values than requested), or
                        // current value beyond fence post, use last known for both cases
                        values.Add(CreateInterpolatedValue(rawValues[rawIndex - 1], fencePost));
                    }
                    else
                    {
                        // found raw value exactly at fence post, use it as is
                        values.Add(rawValues[rawIndex]);
                    }
                    fencePost = fencePost.AddTicks(stepTicks);
                }

                return values;
            }
        }

        private static VQT CreateInterpolatedValue(VQT vtq, DateTime time)
        {
            return new VQT(vtq.Value, time, new Quality() { HDAQuality = HDAQuality.Interpolated });
        }
    }
}
