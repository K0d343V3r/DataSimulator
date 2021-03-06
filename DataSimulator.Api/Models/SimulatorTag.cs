﻿using Newtonsoft.Json;
using NJsonSchema.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    [JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [KnownType(typeof(NumericTag))]
    [KnownType(typeof(BooleanTag))]
    [KnownType(typeof(StringTag))]
    public abstract class SimulatorTag : SimulatorItem
    {
    }
}
