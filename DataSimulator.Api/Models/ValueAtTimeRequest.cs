using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class ValueAtTimeRequest
    {
        public IEnumerable<ItemId> Tags { get; set; }
        public DateTime TargetTime { get; set; }
    }
}
