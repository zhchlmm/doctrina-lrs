using xAPI.Core.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Models
{
    [JsonObject]
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
