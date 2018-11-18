using DataSimulator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Models
{
    public class RelativeHistoryRequest : HistoryRequestBase
    {
        public TimeScale TimeScale { get; set; }
        public int OffsetFromNow { get; set; }
    }
}
