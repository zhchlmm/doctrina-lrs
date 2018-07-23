using Newtonsoft.Json;
using System;

namespace Doctrina.xAPI.Models
{
    public class Verb : IVerb
    {
        /// <summary>
        /// Corresponds to a Verb definition. (Required)
        /// Each Verb definition corresponds to the meaning of a Verb, not the word. 
        /// </summary>
        [JsonProperty("id",
            Required = Required.Always)]
        public Uri Id { get; set; }

        [JsonProperty("display",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public LanguageMap Display { get; set; }
    }
}