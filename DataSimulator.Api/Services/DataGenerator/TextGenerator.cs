using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Services.DataGenerator
{
    public class TextGenerator : DataGenerator
    {
        protected override object GetValueAtTime(DateTime time)
        {
            if (time.Ticks % (TimeSpan.TicksPerSecond * 5) == 0)
            {
                // generate a value every 5 seconds
                return time.ToString("o", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                return null;
            }
        }
    }
}
