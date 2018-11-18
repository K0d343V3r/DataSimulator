using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Models
{
    public class VQT
    {
        public object Value { get; private set; }
        public DateTime Time { get; private set; }
        public Quality Quality { get; private set; }

        public VQT(object value, DateTime time, Quality quality)
        {
            Value = value;
            Time = time;
            Quality = quality;
        }
    }
}
