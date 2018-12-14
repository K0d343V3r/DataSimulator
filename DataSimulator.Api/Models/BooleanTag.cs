using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class BooleanTag : SimulatorTag
    {
        public string TrueLabel { get; set; }
        public string FalseLabel { get; set; }
    }
}
