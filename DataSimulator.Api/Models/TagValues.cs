using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Models
{
    public class TagValues
    {
        public TagId Tag { get; set; }
        public IEnumerable<VQT> Values { get; set; }
    }
}
