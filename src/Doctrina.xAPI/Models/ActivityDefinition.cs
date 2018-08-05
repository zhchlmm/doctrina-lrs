using Newtonsoft.Json;
using System;

namespace Doctrina.xAPI.Models
{
    [JsonObject()]
    public class ActivityDefinition : JsonModel
    {
        [JsonProperty("name", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public LanguageMap Name { get; set; }

        [JsonProperty("description", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public LanguageMap Description { get; set; }

        [JsonProperty("type", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public virtual Uri Type { get; set; }

        /// <summary>
        /// Resolves to a document with human-readable information about the Activity, which could include a way to launch the activity.
        /// </summary>
        [JsonProperty("moreInfo", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Uri MoreInfo { get; set; }

        [JsonProperty("extensions", 
            NullValueHandling = NullValueHandling.Include,
            Required = Required.Default)]
        public Extensions Extentions { get; set; }
    }
}
