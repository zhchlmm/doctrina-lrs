using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Doctrina.xAPI.Json.Converters;

namespace Doctrina.xAPI
{
    public abstract class StatementBase : JsonModel<JObject>, IStatementBase
    {
        public StatementBase() { }

        public StatementBase(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public StatementBase(JObject jobj, ApiVersion version)
        {
            if (jobj["actor"] != null)
            {
                if (jobj["actor"]["objectType"] != null && (string)jobj["actor"]["objectType"] == ObjectType.Group)
                {
                    Actor = new Group(jobj.Value<JObject>("actor"), version);
                }
                else
                {
                    Actor = new Agent(jobj.Value<JObject>("actor"), version);
                }
            }

            if (jobj["verb"] != null)
            {
                Verb = new Verb(jobj.Value<JObject>("verb"), version);
            }

            if (jobj["object"] != null)
            {
                Object = StatementObjectBase.Parse(jobj.Value<JObject>("object"), version);
            }

            if (jobj["result"] != null)
            {
                Result = new Result(jobj.Value<JObject>("result"), version);
            }

            if (jobj["context"] != null)
            {
                Context = new Context(jobj.Value<JObject>("context"), version);
            }

            if (jobj["timestamp"] != null)
            {
                Timestamp = jobj.Value<DateTimeOffset?>("timestamp");
            }

            if (jobj["attachment"] != null)
            {
                Attachments = new AttachmentCollection(jobj.Value<JObject>("attachment"), version);
            }
        }

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
        public IStatementObject Object { get; set; }

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
                   EqualityComparer<IStatementObject>.Default.Equals(Object, @base.Object) &&
                   EqualityComparer<Result>.Default.Equals(Result, @base.Result) &&
                   EqualityComparer<Context>.Default.Equals(Context, @base.Context);
        }

        public override int GetHashCode()
        {
            var hashCode = -1199450156;
            hashCode = hashCode * -1521134295 + EqualityComparer<Agent>.Default.GetHashCode(Actor);
            hashCode = hashCode * -1521134295 + EqualityComparer<Verb>.Default.GetHashCode(Verb);
            hashCode = hashCode * -1521134295 + EqualityComparer<IStatementObject>.Default.GetHashCode(Object);
            hashCode = hashCode * -1521134295 + EqualityComparer<Result>.Default.GetHashCode(Result);
            hashCode = hashCode * -1521134295 + EqualityComparer<Context>.Default.GetHashCode(Context);
            return hashCode;
        }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();

            if (Actor != null)
            {
                jobj["actor"] = Actor.ToJToken(version, format);
            }

            if (Verb != null)
            {
                jobj["verb"] = Verb.ToJToken(version, format);
            }

            if (Object != null)
            {
                jobj["object"] = Object.ToJToken(version, format);
            }

            if (Result != null)
            {
                jobj["result"] = Result.ToJToken(version, format);
            }

            if (Context != null)
            {
                jobj["context"] = Context.ToJToken(version, format);
            }

            if (Timestamp.HasValue)
            {
                jobj["timestamp"] = Timestamp?.ToString("o");
            }

            if (Attachments != null)
            {
                jobj["attachments"] = Attachments.ToJToken(version, format);
            }

            return jobj;
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
