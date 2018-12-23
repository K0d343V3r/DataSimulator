using System;

namespace DataSimulator.Api.Models
{
    public class RelativeHistoryRequest : HistoryRequestBase
    {
        public DateTime? AnchorTime { get; set; }
        public TimeScale TimeScale { get; set; }
        public int OffsetFromNow { get; set; }
    }
}
