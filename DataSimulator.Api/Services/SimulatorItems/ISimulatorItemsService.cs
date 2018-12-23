using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Services.SimulatorItems
{
    public interface ISimulatorItemsService
    {
        IEnumerable<SimulatorItem> GetAllItems();
        SimulatorItem GetItem(ItemId id);
        bool HasDocumentItems(IEnumerable<ItemId> items);
        bool HasTagItems(IEnumerable<ItemId> items);
    }
}
