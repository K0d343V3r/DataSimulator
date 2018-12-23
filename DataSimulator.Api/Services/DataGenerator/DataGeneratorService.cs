using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSimulator.Api.Models;
using DataSimulator.Api.Services.SimulatorItems;

namespace DataSimulator.Api.Services.DataGenerator
{
    public class DataGeneratorService : IDataGeneratorService
    {
        private readonly ISimulatorItemsService _simulatorItemsService;

        public DataGeneratorService(ISimulatorItemsService simulatorItemsService)
        {
            _simulatorItemsService = simulatorItemsService;
        }

        public VQT GenerateValueAt(ItemId id, DateTime time)
        {
            var item = _simulatorItemsService.GetItem(id);
            return CreateGenerator(item).GetValueAt(time);
        }

        private DataGenerator CreateGenerator(SimulatorItem item)
        {
            switch (item.Id)
            {
                case ItemId.SawtoothWave:
                case ItemId.SineWave:
                case ItemId.SquareWave:
                case ItemId.TriangleWave:
                case ItemId.WhiteNoise:
                    {
                        NumericScale scale = ((NumericTag)item).Scale;
                        return new WaveFormGenerator(GetWaveForm(item.Id), scale);
                    }

                case ItemId.IncrementalCount:
                    {
                        NumericScale scale = ((NumericTag)item).Scale;
                        return new CountGenerator(scale);
                    }

                case ItemId.ModulatedPulse:
                case ItemId.PeriodicPulse:
                    return new DiscreteGenerator(item.Id == ItemId.PeriodicPulse);

                case ItemId.TimeText:
                    return new TextGenerator();

                default:
                    throw new InvalidOperationException("Invalid tag id.");
            }
        }

        private WaveForm GetWaveForm(ItemId id)
        {
            switch (id)
            {
                case ItemId.SawtoothWave:
                    return WaveForm.Sawtooth;

                case ItemId.SineWave:
                    return WaveForm.Sine;

                case ItemId.SquareWave:
                    return WaveForm.Square;

                case ItemId.TriangleWave:
                    return WaveForm.Triangle;

                case ItemId.WhiteNoise:
                    return WaveForm.WhiteNoise;

                default:
                    throw new InvalidOperationException("Tag is not a wave form tag.");
            }
        }

        public IList<VQT> GenerateValues(ItemId id, TimePeriod timePeriod, InitialValue initialValue, int maxCount)
        {
            var item = _simulatorItemsService.GetItem(id);
            return CreateGenerator(item).GetValues(timePeriod, initialValue, maxCount);
        }
    }
}
