using DataServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Helpers
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

        private DateTime GetOffsetDate(DateTime now, TimeScale scale, int offsetFromNow)
        {
            DateTime date;
            switch (scale)
            {
                case TimeScale.Days:
                    date = new DateTime(now.Year, now.Month, now.Day + offsetFromNow, now.Hour, now.Minute, now.Second, now.Millisecond);
                    break;

                case TimeScale.Hours:
                    date = new DateTime(now.Year, now.Month, now.Day, now.Hour + offsetFromNow, now.Minute, now.Second, now.Millisecond);
                    break;

                case TimeScale.Milliseconds:
                    date = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond + offsetFromNow);
                    break;

                case TimeScale.Minutes:
                    date = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute + offsetFromNow, now.Second, now.Millisecond);
                    break;

                case TimeScale.Seconds:
                    date = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second + offsetFromNow, now.Millisecond);
                    break;

                default:
                    throw new InvalidOperationException("Invalid time scale.");
            }

            DateTime.SpecifyKind(date, DateTimeKind.Utc);
            return date;
        }
    }
}
