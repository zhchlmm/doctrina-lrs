using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Models.InteractionTypes
{
    public abstract class InteractionTypeBase : ActivityDefinition
    {
        public override Uri Type { get => new Uri("http://adlnet.gov/expapi/activities/cmi.interaction"); set => base.Type = value; }

        protected abstract InteractionType INTERACTION_TYPE { get; }

        [JsonProperty("interactionType",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [EnumDataType(typeof(InteractionType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public InteractionType InteractionType { get { return this.INTERACTION_TYPE; } }

        [JsonProperty("correctResponsesPattern",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public string[] CorrectResponsesPattern { get; set; }
    }
}
