using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Helpers
{
    public static class Items
    {
        public static readonly IEnumerable<SimulatorItem> List;

        static Items()
        {
            List<SimulatorItem> items = new List<SimulatorItem>
            {
                new NumericTag()
                {
                    Id = TagId.SineWave,
                    Name = "Sine Wave",
                    Type = NumericType.Float,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "km"
                },
                new NumericTag()
                {
                    Id = TagId.TriangleWave,
                    Name = "Triangle Wave",
                    Type = NumericType.Float,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "ft/s"
                },
                new NumericTag()
                {
                    Id = TagId.SquareWave,
                    Name = "Square Wave",
                    Type = NumericType.Float,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "m/s"
                },
                new NumericTag()
                {
                    Id = TagId.SawtoothWave,
                    Name = "Sawtooth Wave",
                    Type = NumericType.Float,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "m"
                },
                new NumericTag()
                {
                    Id = TagId.WhiteNoise,
                    Name = "White Noise",
                    Type = NumericType.Float,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "W"
                },
                new NumericTag()
                {
                    Id = TagId.IncrementalCount,
                    Name = "Incremental Count",
                    Type = NumericType.Integer,
                    Scale = new NumericScale(0, 1000),
                    EngineeringUnits = "kg"
                },
                new BooleanTag()
                {
                    Id = TagId.PeriodicPulse,
                    Name = "Periodic Pulse",
                    TrueLabel = "On",
                    FalseLabel = "Off"
                },
                new BooleanTag()
                {
                    Id = TagId.ModulatedPulse,
                    Name = "Modulated Pulse",
                    TrueLabel = "Open",
                    FalseLabel = "Closed"
                },
                new StringTag()
                {
                    Id = TagId.TimeText,
                    Name = "Time Text",
                },
                new SimulatorDocument()
                {
                    Id = TagId.PDFDocument,
                    Name = "PDF Document",
                    MediaType = "application/pdf"
                }
            };

            List = items;
        }

        public static bool HasContent(IEnumerable<TagId> items)
        {
            return items.All(i => i == TagId.PDFDocument);
        }

        public static bool HasTags(IEnumerable<TagId> items)
        {
            return items.All(i => i != TagId.PDFDocument);
        }
    }
}
