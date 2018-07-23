using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctrina.xAPI.Converters;
using System.Globalization;
using Doctrina.xAPI.Schema.Providers;
using Newtonsoft.Json.Schema.Generation;

namespace Doctrina.xAPI.Models
{
    //[JsonConverter(typeof(ContextConverter))]
    [JsonObject]
    public class Context
    {
        [JsonProperty("registration", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Guid? Registration { get; set; }

        /// <summary>
        /// Instructor that the Statement relates to, if not included as the Actor of the Statement.
        /// </summary>
        [JsonProperty("instructor", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull),
            JsonConverter(typeof(AgentJsonConverter))]
        public Agent Instructor { get; set; }

        /// <summary>
        /// Instructor that the Statement relates to, if not included as the Actor of the Statement.
        /// </summary>
        [JsonProperty("team", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull),
            JsonConverter(typeof(AgentJsonConverter))]
        public Group Team { get; set; }

        /// <summary>
        /// A map of the types of learning activity context that this Statement is related to. Valid context types are: parent, "grouping", "category" and "other".
        /// </summary>
        [JsonProperty("contextActivities",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public ContextActivities ContextActivities { get; set; }

        /// <summary>
        /// Revision of the learning activity associated with this Statement. Format is free.
        /// The "revision" property MUST only be used if the Statement's Object is an Activity.
        /// </summary>
        [JsonProperty("revision",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public string Revision { get; set; }

        /// <summary>
        /// Platform used in the experience of this learning activity.
        /// The "platform" property MUST only be used if the Statement's Object is an Activity.
        /// </summary>
        [JsonProperty("platform",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public string Platform { get; set; }

        
        private string _language;
        /// <summary>
        /// Code representing the language in which the experience being recorded in this Statement (mainly) occurred in, if applicable and known.
        /// </summary>
        [JsonProperty("language",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public string Language
        {
            get { return _language; }
            set {
                CultureInfo.GetCultureInfo(value);
                _language = value;
            }
        }


        /// <summary>
        /// Another Statement to be considered as context for this Statement.
        /// </summary>
        [JsonProperty("statement",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public StatementRef Statement { get; set; }

        [JsonProperty("extensions",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.Default)]
        public Extensions Extensions { get; set; }
    }
}
