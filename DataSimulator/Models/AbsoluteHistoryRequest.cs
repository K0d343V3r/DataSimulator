﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Models
{
    public class AbsoluteHistoryRequest : HistoryRequestBase
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
