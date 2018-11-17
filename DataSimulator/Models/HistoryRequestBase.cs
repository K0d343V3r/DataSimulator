using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Models
{
    public abstract class HistoryRequestBase
    {
        public IEnumerable<TagId> Tags { get; set; }
        public InitialValue InitialValue { get; set; }
        public int MaxCount { get; set; }
    }
}
