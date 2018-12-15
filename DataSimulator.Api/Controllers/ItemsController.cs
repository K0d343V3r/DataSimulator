using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataSimulator.Api.Helpers;
using DataSimulator.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataSimulator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimulatorItem>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<SimulatorItem>> GetAllItems()
        {
            return Items.List.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SimulatorItem), (int)HttpStatusCode.OK)]
        public ActionResult<SimulatorItem> GetItem(ItemId id)
        {
            return Items.List.FirstOrDefault(t => t.Id == id);
        }
    }
}
