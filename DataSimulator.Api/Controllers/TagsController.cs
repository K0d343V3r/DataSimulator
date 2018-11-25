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
    public class TagsController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimulatorTag>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<SimulatorTag>> GetAllTags()
        {
            return Tags.List.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SimulatorTag), (int)HttpStatusCode.OK)]
        public ActionResult<SimulatorTag> Get(TagId id)
        {
            return Tags.List.FirstOrDefault(t => t.Id == id);
        }
    }
}
