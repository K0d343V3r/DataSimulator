using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Helpers
{
    public class CountGenerator : DataGenerator
    {
        private readonly NumericScale _scale;

        public CountGenerator(NumericScale scale)
        {
            _scale = scale;
        }

        protected override object GetValueAtTime(DateTime time)
        {
            // return 1 value per second
            return _scale.Min + (time.Ticks / TimeSpan.TicksPerSecond) % (_scale.Max - _scale.Min);
        }
    }
}
