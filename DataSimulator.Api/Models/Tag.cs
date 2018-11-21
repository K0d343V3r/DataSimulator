using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Models
{
    public class Tag
    {
        public TagId Id { get; set; }
        public NumericScale Scale { get; set; }
        public string EngineeringUnits { get; set; }
        public string TrueLabel { get; set; }
        public string FalseLabel { get; set; }
    }
}
