using Doctrina.xAPI.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Models
{
    [JsonObject]
    public class Score : JsonModel
    {
        /// <summary>
        /// The score related to the experience as modified by scaling and/or normalization.
        /// </summary>
        [JsonProperty("scaled", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(NumberConverter))]
        public double? Scaled { get; set; }

        /// <summary>
        /// Decimal number between min and max (if present, otherwise unrestricted), inclusive
        /// The score achieved by the Actor in the experience described by the Statement. This is not modified by any scaling or normalization.
        /// </summary>
        [JsonProperty("raw", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(NumberConverter))]
        public double? Raw { get; set; }

        /// <summary>
        /// Decimal number less than max (if present)
        /// The lowest possible score for the experience described by the Statement.
        /// </summary>
        [JsonProperty("min",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(NumberConverter))]
        public double? Min { get; set; }

        /// <summary>
        /// Decimal number greater than min (if present)
        /// The highest possible score for the experience described by the Statement.
        /// </summary>
        [JsonProperty("max",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(NumberConverter))]
        public double? Max { get; set; }

    }
}
