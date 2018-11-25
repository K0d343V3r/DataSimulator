using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Helpers
{
    public static class Tags
    {
        public static readonly IEnumerable<SimulatorTag> List;

        static Tags()
        {
            List<SimulatorTag> tags = new List<SimulatorTag>
            {
                new SimulatorTag()
                {
                    Id = TagId.SineWave,
                    Name = "Sine Wave",
                    Type = TagType.Number,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "km"
                },
                new SimulatorTag()
                {
                    Id = TagId.TriangleWave,
                    Name = "Triangle Wave",
                    Type = TagType.Number,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "ft/s"
                },
                new SimulatorTag()
                {
                    Id = TagId.SquareWave,
                    Name = "Square Wave",
                    Type = TagType.Number,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "m/s"
                },
                new SimulatorTag()
                {
                    Id = TagId.SawtoothWave,
                    Name = "Sawtooth Wave",
                    Type = TagType.Number,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "m"
                },
                new SimulatorTag()
                {
                    Id = TagId.WhiteNoise,
                    Name = "White Noise",
                    Type = TagType.Number,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "W"
                },
                new SimulatorTag()
                {
                    Id = TagId.IncrementalCount,
                    Name = "Incremental Count",
                    Type = TagType.Number,
                    Scale = new NumericScale(0, 1000),
                    EngineeringUnits = "kg"
                },
                new SimulatorTag()
                {
                    Id = TagId.PeriodicPulse,
                    Name = "Periodic Pulse",
                    Type = TagType.Boolean,
                    TrueLabel = "On",
                    FalseLabel = "Off"
                },
                new SimulatorTag()
                {
                    Id = TagId.ModulatedPulse,
                    Name = "Modulated Pulse",
                    Type = TagType.Boolean,
                    TrueLabel = "Yes",
                    FalseLabel = "No"
                },
                new SimulatorTag()
                {
                    Id = TagId.TimeText,
                    Name = "Time Text",
                    Type = TagType.String
                }
            };

            List = tags;
        }
    }
}
