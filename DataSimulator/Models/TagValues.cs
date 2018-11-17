using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Models
{
    public class TagValues
    {
        public TagId Tag { get; set; }
        public IEnumerable<VQT> Values { get; set; }
    }
}
