using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Services.DataGenerator
{
    public interface IDataGeneratorService
    {
        IList<VQT> GenerateValues(ItemId id, TimePeriod timePeriod, InitialValue initialValue, int maxCount);
        VQT GenerateValueAt(ItemId id, DateTime time);
    }
}
