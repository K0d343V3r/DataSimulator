﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataServices.Helpers;
using DataServices.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Tag>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<Tag>> GetAllTags()
        {
            return Tags.List.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Tag), (int)HttpStatusCode.OK)]
        public ActionResult<Tag> Get(TagId id)
        {
            return Tags.List.FirstOrDefault(t => t.Id == id);
        }
    }
}
