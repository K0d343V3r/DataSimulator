using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataSimulator.Api.Helpers;
using DataSimulator.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataSimulator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentDataController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<string>> GetDocuments([FromBody] IEnumerable<TagId> documents)
        {
            if (Items.HasTags(documents))
            {
                return BadRequest();
            }

            return documents.Select(i => "http://infolab.stanford.edu/pub/papers/google.pdf").ToList();
        }
    }
}