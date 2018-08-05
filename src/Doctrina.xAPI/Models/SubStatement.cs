using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System;

namespace Doctrina.xAPI.Models
{

    // NOTE: A SubStatement MUST NOT have the "id", "stored", "version" or "authority" properties.
    /// <summary>
    /// A SubStatement is like a StatementRef in that it is included as part of a containing Statement, but unlike a StatementRef, it does not represent an event that has occurred. 
    /// It can be used to describe, for example, a predication of a potential future Statement or the behavior a teacher looked for when evaluating a student (without representing the student actually doing that behavior)
    /// SubStatement <see cref="https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#substatements"/>
    /// </summary>

    [JsonObject]
    public class SubStatement : StatementTargetBase
    {
        protected override ObjectType OBJECT_TYPE => ObjectType.SubStatement;

        public SubStatement() : base() { }

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
        /// Activity, Agent, or StatementRef (SubStatements are not allowed to nest)
        /// </summary>
        [JsonProperty("object",
            Order = 5,
            Required = Required.Always)]
        [JsonConverter(typeof(StatementTargetConverter))]
        public StatementTargetBase Target { get; set; }

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
