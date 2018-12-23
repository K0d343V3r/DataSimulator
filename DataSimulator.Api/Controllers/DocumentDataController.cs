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
    public class DocumentDataController : ControllerBase
    {
        private readonly ISimulatorItemsService _simulatorItemsService;

        public DocumentDataController(ISimulatorItemsService simulatorItemsService)
        {
            _simulatorItemsService = simulatorItemsService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<DocumentValue>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<DocumentValue>> GetDocumentValues([FromBody] IEnumerable<ItemId> documents)
        {
            if (_simulatorItemsService.HasTagItems(documents))
            {
                return BadRequest();
            }
            else
            {
                return documents.Select(i => new DocumentValue()
                {
                    Document = i,
                    Url = "http://infolab.stanford.edu/pub/papers/google.pdf"
                }).ToList();
            }
        }
    }
}