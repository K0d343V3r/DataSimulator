using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class HistoryResponse
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public IEnumerable<TagValues> Values;
    }
}
