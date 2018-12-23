using DataSimulator.Api.Models;
using DataSimulator.Api.Services.DataGenerator;
using DataSimulator.Api.Services.SimulatorItems;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DataSimulator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagDataController : ControllerBase
    {
        private readonly IDataGeneratorService _dataGeneratorService;
        private readonly ISimulatorItemsService _simulatorItemsService;

        public TagDataController(
            IDataGeneratorService dataGeneratorService,
            ISimulatorItemsService simulatorItemsService
        )
        {
            _dataGeneratorService = dataGeneratorService;
            _simulatorItemsService = simulatorItemsService;
        }

        [HttpPost("history/absolute")]
        [ProducesResponseType(typeof(HistoryResponse), (int)HttpStatusCode.OK)]
        public ActionResult<HistoryResponse> GetHistoryAbsolute([FromBody] AbsoluteHistoryRequest request)
        {
            if (_simulatorItemsService.HasDocumentItems(request.Tags))
            {
                return BadRequest();
            }
            else
            {
                TimePeriod timePeriod = new TimePeriod(request.StartTime, request.EndTime);
                return GetHistory(timePeriod, request as HistoryRequestBase);
            }
        }

        private HistoryResponse GetHistory(TimePeriod timePeriod, HistoryRequestBase request)
        {
            List<TagValues> results = new List<TagValues>();
            foreach (var id in request.Tags)
            {
                var values = new TagValues { Tag = id };
                values.Values = _dataGeneratorService.GenerateValues(id, timePeriod, request.InitialValue, request.MaxCount);
                results.Add(values);
            }

            return new HistoryResponse()
            {
                StartTime = timePeriod.StartTime,
                EndTime = timePeriod.EndTime,
                Values = results
            };
        }

        [HttpPost("history/relative")]
        [ProducesResponseType(typeof(HistoryResponse), (int)HttpStatusCode.OK)]
        public ActionResult<HistoryResponse> GetHistoryRelative([FromBody] RelativeHistoryRequest request)
        {
            if (_simulatorItemsService.HasDocumentItems(request.Tags))
            {
                return BadRequest();
            }
            else
            {
                TimePeriod timePeriod = request.AnchorTime.HasValue ?
                    new TimePeriod(request.AnchorTime.Value) :
                    new TimePeriod(request.TimeScale, request.OffsetFromNow);
                return GetHistory(timePeriod, request as HistoryRequestBase);
            }
        }

        [HttpPost("valueattime")]
        [ProducesResponseType(typeof(IEnumerable<TagValue>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<TagValue>> GetValueAtTime([FromBody] ValueAtTimeRequest request)
        {
            if (_simulatorItemsService.HasDocumentItems(request.Tags))
            {
                return BadRequest();
            }
            else
            {
                return GetValuesAtTime(request.Tags, request.TargetTime);
            }
        }

        private List<TagValue> GetValuesAtTime(IEnumerable<ItemId> tags, DateTime targetTime)
        {
            List<TagValue> values = new List<TagValue>();
            foreach (var tag in tags)
            {
                values.Add(new TagValue()
                {
                    Tag = tag,
                    Value = _dataGeneratorService.GenerateValueAt(tag, targetTime)
                });
            }

            return values;
        }

        [HttpPost("livevalue")]
        [ProducesResponseType(typeof(IEnumerable<TagValue>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<TagValue>> GetLiveValue([FromBody] IEnumerable<ItemId> tags)
        {
            if (_simulatorItemsService.HasDocumentItems(tags))
            {
                return BadRequest();
            }
            else
            {
                return GetValuesAtTime(tags, DateTime.UtcNow);
            }
        }
    }
}
