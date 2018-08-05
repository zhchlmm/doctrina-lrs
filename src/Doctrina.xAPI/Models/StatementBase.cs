using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System;

namespace Doctrina.xAPI.Models
{
    public abstract class StatementBase : JsonModel
    {
        /// <summary>
        /// Whom the Statement is about, as an Agent or Group Object.
        /// </summary>
        [JsonProperty("actor",
            Required = Required.Always,
            Order = 3)]
        [JsonConverter(typeof(AgentJsonConverter))]
        public Agent Actor { get; set; }

        /// <summary>
        /// Action taken by the Actor.
        /// </summary>
        [JsonProperty("verb",
            Order = 4,
            Required = Required.Always)]
        public Verb Verb { get; set; }

        /// <summary>
        /// Activity, Agent, or another Statement that is the Object of the Statement.
        /// </summary>
        [JsonProperty("object",
            Order = 5,
            Required = Required.Always)]
        [JsonConverter(typeof(StatementTargetConverter))]
        public StatementTargetBase Object { get; set; }

        /// <summary>
        /// Result Object, further details representing a measured outcome.
        /// </summary>
        [JsonProperty("result",
            Order = 6,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public Result Result { get; set; }

        /// <summary>
        /// Context that gives the Statement more meaning. Examples: a team the Actor is working with, altitude at which a scenario was attempted in a flight simulator.
        /// </summary>
        [JsonProperty("context",
            Order = 7,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public Context Context { get; set; }

        /// <summary>
        /// Timestamp of when the events described within this Statement occurred. Set by the LRS if not provided.
        /// </summary>
        [JsonProperty("timestamp",
            Order = 8,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Timestamp { get; set; }


        [JsonProperty("attachments",
            Order = 9,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public Attachment[] Attachments { get; set; }
    }
}
