using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Models
{
    public class TagValue
    {
        public TagId Tag { get; set; }
        public VQT Value { get; set; }
    }
}
