using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataServices.Helpers;
using DataServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpPost("history/absolute")]
        [ProducesResponseType(typeof(IEnumerable<TagValues>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<TagValues>> GetHistoryAbsolute([FromBody] AbsoluteHistoryRequest options)
        {
            TimePeriod timePeriod = new TimePeriod(options.StartTime, options.EndTime);
            return GetHistory(timePeriod, options as HistoryRequestBase);
        }

        private List<TagValues> GetHistory(TimePeriod timePeriod, HistoryRequestBase request)
        {
            List<TagValues> results = new List<TagValues>();
            foreach (var id in request.Tags)
            {
                var values = new TagValues { Tag = id };
                var generator = CreateGenerator(id);
                IList<VQT> rawValues = generator.GetValues(timePeriod, request.InitialValue);
                if (rawValues.Count > request.MaxCount)
                {
                    values.Values = DataAggregator.InterpolateValues(rawValues, timePeriod, request.MaxCount);
                }
                else
                {
                    values.Values = rawValues;
                }
                results.Add(values);
            }

            return results;
        }

        private DataGenerator CreateGenerator(TagId tag)
        {
            switch (tag)
            {
                case TagId.NumericSawtooth:
                case TagId.NumericSine:
                case TagId.NumericSquare:
                case TagId.NumericTriangle:
                case TagId.NumericWhiteNoise:
                    NumericScale scale = Tags.List.First(t => t.Id == tag).Scale;
                    return new WaveFormGenerator(GetWaveForm(tag), scale);

                default:
                    throw new InvalidOperationException("Invalid tag id.");
            }
        }

        private WaveForm GetWaveForm(TagId id)
        {
            switch (id)
            {
                case TagId.NumericSawtooth:
                    return WaveForm.Sawtooth;

                case TagId.NumericSine:
                    return WaveForm.Sine;

                case TagId.NumericSquare:
                    return WaveForm.Square;

                case TagId.NumericTriangle:
                    return WaveForm.Triangle;

                case TagId.NumericWhiteNoise:
                    return WaveForm.WhiteNoise;

                default:
                    throw new InvalidOperationException("Tag is not a wave form tag.");
            }
        }

        [HttpPost("history/relative")]
        [ProducesResponseType(typeof(IEnumerable<TagValues>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<TagValues>> GetHistoryRelative([FromBody] RelativeHistoryRequest request)
        {
            TimePeriod timePeriod = new TimePeriod(request.TimeScale, request.OffsetFromNow);
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
