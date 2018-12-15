using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class TagValues
    {
        public ItemId Tag { get; set; }
        public IEnumerable<VQT> Values { get; set; }
    }
}
