using DataServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServices.Helpers
{
    public static class Tags
    {
        public static readonly IEnumerable<Tag> List;

        static Tags()
        {
            List<Tag> tags = new List<Tag>
            {
                new Tag()
                {
                    Id = TagId.NumericSawtooth,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "m"
                },
                new Tag()
                {
                    Id = TagId.NumericSine,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "km"
                },
                new Tag()
                {
                    Id = TagId.NumericSquare,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "m/s"
                },
                new Tag()
                {
                    Id = TagId.NumericTriangle,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "ft/s"
                },
                new Tag()
                {
                    Id = TagId.NumericWhiteNoise,
                    Scale = new NumericScale(0, 100),
                    EngineeringUnits = "W"
                }
            };

            List = tags;
        }
    }
}
