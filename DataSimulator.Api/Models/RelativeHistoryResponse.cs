using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class RelativeHistoryResponse
    {
        public DateTime ResolvedStartTime;
        public DateTime ResolvedEndTime;
        public IEnumerable<TagValues> Values;
    }
}
