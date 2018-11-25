using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class SimulatorTag
    {
        public TagId Id { get; set; }
        public string Name { get; set; }
        public TagType Type { get; set; }
        public NumericScale Scale { get; set; }
        public string EngineeringUnits { get; set; }
        public string TrueLabel { get; set; }
        public string FalseLabel { get; set; }
    }
}
