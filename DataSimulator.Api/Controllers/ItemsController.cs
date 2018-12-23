using DataSimulator.Api.Models;
using DataSimulator.Api.Services.SimulatorItems;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DataSimulator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ISimulatorItemsService _simulatorItemsService;

        public ItemsController(ISimulatorItemsService simulatorItemsService)
        {
            _simulatorItemsService = simulatorItemsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimulatorItem>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<SimulatorItem>> GetAllItems()
        {
            return _simulatorItemsService.GetAllItems().ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SimulatorItem), (int)HttpStatusCode.OK)]
        public ActionResult<SimulatorItem> GetItem(ItemId id)
        {
            return _simulatorItemsService.GetItem(id);
        }
    }
}
