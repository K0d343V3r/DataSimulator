using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class NumericTag : SimulatorTag
    {
        public NumericType Type { get; set; }
        public NumericScale Scale { get; set; }
        public string EngineeringUnits { get; set; }
    }
}
