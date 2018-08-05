using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System;

namespace Doctrina.xAPI.Models
{
    [JsonObject]
    public class Result : JsonModel
    {
        /// <summary>
        /// The score of the Agent in relation to the success or quality of the experience. See: <seealso cref="Models.Score"/>
        /// </summary>
        /// 
        [JsonProperty("score",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Score Score { get; set; }

        /// <summary>
        /// Indicates whether or not the attempt on the Activity was successful
        /// </summary>
        [JsonProperty("success",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(BooleanConverter))]
        public bool? Success { get; set; }

        /// <summary>
        /// Indicates whether or not the Activity was completed.
        /// </summary>
        [JsonProperty("completion",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(BooleanConverter))]
        public bool? Completion { get; set; }


        /// <summary>
        /// A response appropriately formatted for the given Activity.
        /// </summary>
        [JsonProperty("response",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public string Response { get; set; }

        /// <summary>
        /// Period of time over which the Statement occurred.
        /// </summary>
        [JsonProperty("duration",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// A map of other properties as needed. See: <seealso cref="Models.Extensions"/>
        /// </summary>
        [JsonProperty("extensions",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.Default)]
        public Extensions Extentions { get; set; }
    }
}
