using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Models
{
    public class ValueAtTimeRequest
    {
        public IEnumerable<TagId> Tags { get; set; }
        public DateTime TargetTime { get; set; }
    }
}
