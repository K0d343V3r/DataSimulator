using DataSimulator.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class RelativeHistoryRequest : HistoryRequestBase
    {
        public TimeScale TimeScale { get; set; }
        public int OffsetFromNow { get; set; }
    }
}
