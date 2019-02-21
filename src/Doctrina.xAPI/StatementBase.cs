using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
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
        [JsonConverter(typeof(StatementObjectConverter))]
        public StatementObjectBase Object { get; set; }

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
        public DateTimeOffset? Timestamp { get; set; }

        [JsonProperty("attachments",
            Order = 9,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public AttachmentCollection Attachments { get; set; }

        public override bool Equals(object obj)
        {
            var @base = obj as StatementBase;
            return @base != null &&
                   base.Equals(obj) &&
                   EqualityComparer<Agent>.Default.Equals(Actor, @base.Actor) &&
                   EqualityComparer<Verb>.Default.Equals(Verb, @base.Verb) &&
                   EqualityComparer<StatementObjectBase>.Default.Equals(Object, @base.Object) &&
                   EqualityComparer<Result>.Default.Equals(Result, @base.Result) &&
                   EqualityComparer<Context>.Default.Equals(Context, @base.Context);
        }

        public override int GetHashCode()
        {
            var hashCode = -1199450156;
            hashCode = hashCode * -1521134295 + EqualityComparer<Agent>.Default.GetHashCode(Actor);
            hashCode = hashCode * -1521134295 + EqualityComparer<Verb>.Default.GetHashCode(Verb);
            hashCode = hashCode * -1521134295 + EqualityComparer<StatementObjectBase>.Default.GetHashCode(Object);
            hashCode = hashCode * -1521134295 + EqualityComparer<Result>.Default.GetHashCode(Result);
            hashCode = hashCode * -1521134295 + EqualityComparer<Context>.Default.GetHashCode(Context);
            return hashCode;
        }

        public static bool operator ==(StatementBase base1, StatementBase base2)
        {
            return EqualityComparer<StatementBase>.Default.Equals(base1, base2);
        }

        public static bool operator !=(StatementBase base1, StatementBase base2)
        {
            return !(base1 == base2);
        }
    }
}
