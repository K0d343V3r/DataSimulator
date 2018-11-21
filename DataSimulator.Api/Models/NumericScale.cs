using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class NumericScale
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public NumericScale(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min");
            }
            Min = min;
            Max = max;
        }
    }
}
