using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Services.SimulatorItems
{
    public class SimulatorItemsService : ISimulatorItemsService
    {
        private readonly IList<SimulatorItem> _list = new List<SimulatorItem>
        {
            new NumericTag()
            {
                Id = ItemId.SineWave,
                Name = "Sine Wave",
                Type = NumericType.Float,
                Scale = new NumericScale(0, 100),
                EngineeringUnits = "km"
            },
            new NumericTag()
            {
                Id = ItemId.TriangleWave,
                Name = "Triangle Wave",
                Type = NumericType.Float,
                Scale = new NumericScale(0, 100),
                EngineeringUnits = "ft/s"
            },
            new NumericTag()
            {
                Id = ItemId.SquareWave,
                Name = "Square Wave",
                Type = NumericType.Float,
                Scale = new NumericScale(0, 100),
                EngineeringUnits = "m/s"
            },
            new NumericTag()
            {
                Id = ItemId.SawtoothWave,
                Name = "Sawtooth Wave",
                Type = NumericType.Float,
                Scale = new NumericScale(0, 100),
                EngineeringUnits = "m"
            },
            new NumericTag()
            {
                Id = ItemId.WhiteNoise,
                Name = "White Noise",
                Type = NumericType.Float,
                Scale = new NumericScale(0, 100),
                EngineeringUnits = "W"
            },
            new NumericTag()
            {
                Id = ItemId.IncrementalCount,
                Name = "Incremental Count",
                Type = NumericType.Integer,
                Scale = new NumericScale(0, 1000),
                EngineeringUnits = "kg"
            },
            new BooleanTag()
            {
                Id = ItemId.PeriodicPulse,
                Name = "Periodic Pulse",
                TrueLabel = "On",
                FalseLabel = "Off"
            },
            new BooleanTag()
            {
                Id = ItemId.ModulatedPulse,
                Name = "Modulated Pulse",
                TrueLabel = "Open",
                FalseLabel = "Closed"
            },
            new StringTag()
            {
                Id = ItemId.TimeText,
                Name = "Time Text",
            },
            new SimulatorDocument()
            {
                Id = ItemId.PDFDocument,
                Name = "PDF Document",
                MediaType = "application/pdf"
            }
        };

        public IEnumerable<SimulatorItem> GetAllItems()
        {
            return _list;
        }

        public SimulatorItem GetItem(ItemId id)
        {
            return _list.First(i => i.Id == id);
        }

        public bool HasDocumentItems(IEnumerable<ItemId> items)
        {
            return items.Any(i => i == ItemId.PDFDocument);
        }

        public bool HasTagItems(IEnumerable<ItemId> items)
        {
            return items.Any(i => i != ItemId.PDFDocument);
        }
    }
}
