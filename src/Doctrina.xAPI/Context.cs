using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Doctrina.xAPI
{
    //[JsonConverter(typeof(ContextConverter))]
    [JsonObject]
    public class Context : JsonModel
    {
        public Context()
        {
        }

        public Context(string jsonString) : this(JObject.Parse(jsonString))
        {
        }

        public Context(JObject jobj) : this(jobj, ApiVersion.GetLatest())
        {
        }

        public Context(JObject jobj, ApiVersion version)
        {
            if (jobj["registration"] != null)
            {
                Registration = jobj.Value<Guid?>("registration");
            }

            if (jobj["instructor"] != null)
            {
                Instructor = (Agent)Agent.Parse(jobj.Value<JObject>("instructor"), version);
            }

            if (jobj["team"] != null)
            {
                Instructor = (Group)Group.Parse(jobj.Value<JObject>("team"), version);
            }

            if (jobj["contextActivities"] != null)
            {
                ContextActivities = new ContextActivities(jobj.Value<JObject>("contextActivities"), version);
            }

            if (jobj["revision"] != null)
            {
                Revision = jobj.Value<string>("revision");
            }

            if (jobj["platform"] != null)
            {
                Platform = jobj.Value<string>("platform");
            }

            if (jobj["language"] != null)
            {
                Language = jobj.Value<string>("language");
            }

            if (jobj["statement"] != null)
            {
                Statement = new StatementRef(jobj.Value<JObject>("statement"), version);
            }

            if (jobj["extensions"] != null)
            {
                Extensions = new Extensions(jobj.Value<JObject>("extensions"), version);
            }
        }

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
            set
            {
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

        public override bool Equals(object obj)
        {
            var context = obj as Context;
            return context != null &&
                   EqualityComparer<Guid?>.Default.Equals(Registration, context.Registration) &&
                   EqualityComparer<Agent>.Default.Equals(Instructor, context.Instructor) &&
                   EqualityComparer<Group>.Default.Equals(Team, context.Team) &&
                   EqualityComparer<ContextActivities>.Default.Equals(ContextActivities, context.ContextActivities) &&
                   Revision == context.Revision &&
                   Platform == context.Platform &&
                   Language == context.Language &&
                   EqualityComparer<StatementRef>.Default.Equals(Statement, context.Statement) &&
                   EqualityComparer<Extensions>.Default.Equals(Extensions, context.Extensions);
        }

        public override int GetHashCode()
        {
            var hashCode = 2045218665;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid?>.Default.GetHashCode(Registration);
            hashCode = hashCode * -1521134295 + EqualityComparer<Agent>.Default.GetHashCode(Instructor);
            hashCode = hashCode * -1521134295 + EqualityComparer<Group>.Default.GetHashCode(Team);
            hashCode = hashCode * -1521134295 + EqualityComparer<ContextActivities>.Default.GetHashCode(ContextActivities);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Revision);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Platform);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Language);
            hashCode = hashCode * -1521134295 + EqualityComparer<StatementRef>.Default.GetHashCode(Statement);
            hashCode = hashCode * -1521134295 + EqualityComparer<Extensions>.Default.GetHashCode(Extensions);
            return hashCode;
        }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();
            if (Registration.HasValue)
            {
                jobj["registration"] = Registration;
            }

            if (Instructor != null)
            {
                jobj["instructor"] = Instructor.ToJToken(version, format);
            }

            if (Instructor != null)
            {
                jobj["instructor"] = Instructor.ToJToken(version, format);
            }

            if (Team != null)
            {
                jobj["team"] = Team.ToJToken(version, format);
            }

            if (ContextActivities != null)
            {
                jobj["contextActivities"] = ContextActivities.ToJToken(version, format);
            }

            if (Revision != null)
            {
                jobj["revision"] = Revision;
            }

            if (Platform != null)
            {
                jobj["platform"] = Platform;
            }

            if (Language != null)
            {
                jobj["language"] = Language;
            }

            if (Statement != null)
            {
                jobj["statement"] = Statement.ToJToken(version, format);
            }

            if (Extensions != null)
            {
                jobj["extensions"] = Extensions.ToJToken(version, format);
            }

            return jobj;
        }

        public static bool operator ==(Context context1, Context context2)
        {
            return EqualityComparer<Context>.Default.Equals(context1, context2);
        }

        public static bool operator !=(Context context1, Context context2)
        {
            return !(context1 == context2);
        }
    }
}
