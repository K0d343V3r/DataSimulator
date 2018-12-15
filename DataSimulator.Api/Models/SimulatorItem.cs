using Newtonsoft.Json;
using NJsonSchema.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DataSimulator.Api.Models
{
    [JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [KnownType(typeof(SimulatorTag))]
    [KnownType(typeof(SimulatorDocument))]
    public abstract class SimulatorItem
    {
        public ItemId Id { get; set; }
        public string Name { get; set; }
    }
}
