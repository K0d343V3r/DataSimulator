using DataServices.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Models
{
    public class RelativeHistoryRequest : HistoryRequestBase
    {
        public TimeScale TimeScale { get; set; }
        public int OffsetFromNow { get; set; }
    }
}
