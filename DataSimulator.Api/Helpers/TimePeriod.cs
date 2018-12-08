using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Helpers
{
    public class TimePeriod
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public TimeSpan TimeSpan
        {
            get
            {
                return EndTime - StartTime;
            }
        }

        public TimePeriod(DateTime startTime, DateTime endTime)
        {
            DateTime utcStartTime = startTime.ToUniversalTime();
            DateTime utcEndTime = endTime.ToUniversalTime();

            if (startTime > endTime)
            {
                throw new ArgumentOutOfRangeException("startTime");
            }
            else
            {
                StartTime = startTime;
                EndTime = endTime;
            }
        }

        public TimePeriod(TimeScale scale, int offsetFromNow)
        {
            DateTime now = DateTime.UtcNow;
            DateTime date = GetOffsetDate(now, scale, offsetFromNow);
            if (offsetFromNow <= 0)
            {
                StartTime = date;
                EndTime = now;
            }
            else
            {
                StartTime = now;
                EndTime = date;
            }
        }

        public TimePeriod(DateTime anchorTime)
        {
            DateTime now = DateTime.UtcNow;
            DateTime utcAnchorTime = anchorTime.ToUniversalTime();
            if (anchorTime <= now)
            {
                StartTime = anchorTime;
                EndTime = now;
            }
            else
            {
                StartTime = now;
                EndTime = utcAnchorTime;
            }
        }

        private DateTime GetOffsetDate(DateTime now, TimeScale scale, int offsetFromNow)
        {
            long ticks;
            switch (scale)
            {
                case TimeScale.Days:
                    ticks = offsetFromNow * TimeSpan.TicksPerDay;
                    break;

                case TimeScale.Hours:
                    ticks = offsetFromNow * TimeSpan.TicksPerHour;
                    break;

                case TimeScale.Minutes:
                    ticks = offsetFromNow * TimeSpan.TicksPerMinute;
                    break;

                case TimeScale.Seconds:
                    ticks = offsetFromNow * TimeSpan.TicksPerSecond;
                    break;

                default:
                    throw new InvalidOperationException("Invalid time scale.");
            }

            return now.AddTicks(ticks);
        }
    }
}
