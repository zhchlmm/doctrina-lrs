using Doctrina.xAPI.Json.Converters;
using Doctrina.xAPI.Schema.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using System;

namespace Doctrina.xAPI.Models
{
    [JsonObject]
    [JSchemaGenerationProvider(typeof(ActivitySchemaProvider))]
    public class Activity : StatementTargetBase
    {
        protected override ObjectType OBJECT_TYPE => ObjectType.Activity;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id",
            Required = Required.Always)]
        public Uri Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("definition", 
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ActivityDefinitionConverter))]
        public ActivityDefinition Definition { get; set; }
    }
}
