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
    public class DataController : ControllerBase
    {
        [HttpPost("history/absolute")]
        [ProducesResponseType(typeof(HistoryResponse), (int)HttpStatusCode.OK)]
        public ActionResult<HistoryResponse> GetHistoryAbsolute([FromBody] AbsoluteHistoryRequest options)
        {
            TimePeriod timePeriod = new TimePeriod(options.StartTime, options.EndTime);
            return GetHistory(timePeriod, options as HistoryRequestBase);
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

        private DataGenerator CreateGenerator(TagId tag)
        {
            switch (tag)
            {
                case TagId.SawtoothWave:
                case TagId.SineWave:
                case TagId.SquareWave:
                case TagId.TriangleWave:
                case TagId.WhiteNoise:
                    {
                        NumericScale scale = Tags.List.First(t => t.Id == tag).Scale;
                        return new WaveFormGenerator(GetWaveForm(tag), scale);
                    }

                case TagId.IncrementalCount:
                    {
                        NumericScale scale = Tags.List.First(t => t.Id == tag).Scale;
                        return new CountGenerator(scale);
                    }

                case TagId.ModulatedPulse:
                case TagId.PeriodicPulse:
                    return new DiscreteGenerator(tag == TagId.PeriodicPulse);

                case TagId.TimeText:
                    return new TextGenerator();

                default:
                    throw new InvalidOperationException("Invalid tag id.");
            }
        }

        private WaveForm GetWaveForm(TagId id)
        {
            switch (id)
            {
                case TagId.SawtoothWave:
                    return WaveForm.Sawtooth;

                case TagId.SineWave:
                    return WaveForm.Sine;

                case TagId.SquareWave:
                    return WaveForm.Square;

                case TagId.TriangleWave:
                    return WaveForm.Triangle;

                case TagId.WhiteNoise:
                    return WaveForm.WhiteNoise;

                default:
                    throw new InvalidOperationException("Tag is not a wave form tag.");
            }
        }

        [HttpPost("history/relative")]
        [ProducesResponseType(typeof(HistoryResponse), (int)HttpStatusCode.OK)]
        public ActionResult<HistoryResponse> GetHistoryRelative([FromBody] RelativeHistoryRequest request)
        {
            TimePeriod timePeriod = request.AnchorTime.HasValue ?
                new TimePeriod(request.AnchorTime.Value) :
                new TimePeriod(request.TimeScale, request.OffsetFromNow);
            return GetHistory(timePeriod, request as HistoryRequestBase);
        }

        [HttpPost("valueattime")]
        [ProducesResponseType(typeof(IEnumerable<TagValue>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<TagValue>> GetValueAtTime([FromBody] ValueAtTimeRequest request)
        {
            return GetValuesAtTime(request.Tags, request.TargetTime);
        }

        private List<TagValue> GetValuesAtTime(IEnumerable<TagId> tags, DateTime targetTime)
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
        public ActionResult<IEnumerable<TagValue>> GetLiveValue([FromBody] IEnumerable<TagId> tags)
        {
            return GetValuesAtTime(tags, DateTime.UtcNow);
        }
    }
}
