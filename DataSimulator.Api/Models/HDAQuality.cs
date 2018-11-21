using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    public enum HDAQuality
    {
        None,
        ExtraData,
        Interpolated,
        Raw,
        Calculated,
        NoBound,
        NoData,
        DataLost,
        Conversion,
        Partial
    }
}
