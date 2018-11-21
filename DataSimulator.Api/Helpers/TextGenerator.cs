using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Helpers
{
    public class TextGenerator : DataGenerator
    {
        protected override object GetValueAtTime(DateTime time)
        {
            return time.ToString("o", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
