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
    public class TagDataController : ControllerBase
    {
        [HttpPost("history/absolute")]
        [ProducesResponseType(typeof(HistoryResponse), (int)HttpStatusCode.OK)]
        public ActionResult<HistoryResponse> GetHistoryAbsolute([FromBody] AbsoluteHistoryRequest request)
        {
            if (Items.HasContent(request.Tags))
            {
                return BadRequest();
            }

            TimePeriod timePeriod = new TimePeriod(request.StartTime, request.EndTime);
            return GetHistory(timePeriod, request as HistoryRequestBase);
        }

        private HistoryResponse GetHistory(TimePeriod timePeriod, HistoryRequestBase request)
        {
            List<TagValues> results = new List<TagValues>();
            foreach (var id in request.Tags)
            {
                var values = new TagValues { Tag = id };
                var generator = CreateGenerator(id);
                values.Values = generator.GetValues(timePeriod, request.InitialValue, request.MaxCount);
                results.Add(values);
            }

            return new HistoryResponse()
            {
                StartTime = timePeriod.StartTime,
                EndTime = timePeriod.EndTime,
                Values = results
            };
        }

        private DataGenerator CreateGenerator(ItemId tag)
        {
            switch (tag)
            {
                case ItemId.SawtoothWave:
                case ItemId.SineWave:
                case ItemId.SquareWave:
                case ItemId.TriangleWave:
                case ItemId.WhiteNoise:
                    {
                        NumericScale scale = ((NumericTag)Items.List.First(t => t.Id == tag)).Scale;
                        return new WaveFormGenerator(GetWaveForm(tag), scale);
                    }

                case ItemId.IncrementalCount:
                    {
                        NumericScale scale = ((NumericTag)Items.List.First(t => t.Id == tag)).Scale;
                        return new CountGenerator(scale);
                    }

                case ItemId.ModulatedPulse:
                case ItemId.PeriodicPulse:
                    return new DiscreteGenerator(tag == ItemId.PeriodicPulse);

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

        [HttpPost("history/relative")]
        [ProducesResponseType(typeof(HistoryResponse), (int)HttpStatusCode.OK)]
        public ActionResult<HistoryResponse> GetHistoryRelative([FromBody] RelativeHistoryRequest request)
        {
            if (Items.HasContent(request.Tags))
            {
                return BadRequest();
            }

            TimePeriod timePeriod = request.AnchorTime.HasValue ?
                new TimePeriod(request.AnchorTime.Value) :
                new TimePeriod(request.TimeScale, request.OffsetFromNow);
            return GetHistory(timePeriod, request as HistoryRequestBase);
        }

        [HttpPost("valueattime")]
        [ProducesResponseType(typeof(IEnumerable<TagValue>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<TagValue>> GetValueAtTime([FromBody] ValueAtTimeRequest request)
        {
            if (Items.HasContent(request.Tags))
            {
                return BadRequest();
            }

            return GetValuesAtTime(request.Tags, request.TargetTime);
        }

        private List<TagValue> GetValuesAtTime(IEnumerable<ItemId> tags, DateTime targetTime)
        {
            List<TagValue> values = new List<TagValue>();
            foreach (var tag in tags)
            {
                var generator = CreateGenerator(tag);
                values.Add(new TagValue() { Tag = tag, Value = generator.GetValueAt(targetTime) });
            }

            return values;
        }

        [HttpPost("livevalue")]
        [ProducesResponseType(typeof(IEnumerable<TagValue>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<TagValue>> GetLiveValue([FromBody] IEnumerable<ItemId> tags)
        {
            if (Items.HasContent(tags))
            {
                return BadRequest();
            }

            return GetValuesAtTime(tags, DateTime.UtcNow);
        }
    }
}
