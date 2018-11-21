using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public class Quality
    {
        public MajorQuality Major { get; set; } = MajorQuality.Good;
        public HDAQuality HDAQuality { get; set; } = HDAQuality.Raw;
    }
}
