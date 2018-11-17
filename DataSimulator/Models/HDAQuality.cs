﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Models
{
    public enum HDAQuality
    {
        None,
        ExtraData,
        Interpolated,
        Raw,
        Calculated,
        NoBound,
        DataLost,
        Conversion,
        Partial
    }
}
