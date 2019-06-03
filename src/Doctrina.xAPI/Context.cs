using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Doctrina.xAPI
{
    public class Context : JsonModel
    {
        public Context()
        {
        }

        public Context(JsonString jsonString) : this(jsonString.ToJToken())
        {
        }

        public Context(JToken jobj) : this(jobj, ApiVersion.GetLatest())
        {
        }

        public Context(JToken jobj, ApiVersion version)
        {
            if (!AllowObject(jobj))
            {
                return;
            }

            if (DisallowNullValue(jobj["registration"]) && AllowString(jobj["registration"]))
            {
                Registration = Guid.Parse(jobj.Value<string>("registration"));
            }

            if (DisallowNullValue(jobj["instructor"]))
            {
                Instructor = new Agent(jobj["instructor"], version);
            }

            if (DisallowNullValue(jobj["team"]))
            {
                Instructor = new Group(jobj["team"], version);
            }

            if (DisallowNullValue(jobj["contextActivities"]))
            {
                ContextActivities = new ContextActivities(jobj["contextActivities"], version);
            }

            if (DisallowNullValue(jobj["revision"]) && AllowString(jobj["revision"]))
            {
                Revision = jobj.Value<string>("revision");
            }

            if (DisallowNullValue(jobj["platform"]) && AllowString(jobj["platform"]))
            {
                Platform = jobj.Value<string>("platform");
            }

            if (DisallowNullValue(jobj["language"]) && AllowString(jobj["language"]))
            {
                Language = jobj.Value<string>("language");
            }

            if (DisallowNullValue(jobj["statement"]))
            {
                Statement = new StatementRef(jobj["statement"], version);
            }

            if (DisallowNullValue(jobj["extensions"]))
            {
                Extensions = new Extensions(jobj["extensions"], version);
            }
        }

        public Guid? Registration { get; set; }

        /// <summary>
        /// Instructor that the Statement relates to, if not included as the Actor of the Statement.
        /// </summary>
        public Agent Instructor { get; set; }

        /// <summary>
        /// Instructor that the Statement relates to, if not included as the Actor of the Statement.
        /// </summary>
        public Group Team { get; set; }

        /// <summary>
        /// A map of the types of learning activity context that this Statement is related to. Valid context types are: parent, "grouping", "category" and "other".
        /// </summary>
        public ContextActivities ContextActivities { get; set; }

        /// <summary>
        /// Revision of the learning activity associated with this Statement. Format is free.
        /// The "revision" property MUST only be used if the Statement's Object is an Activity.
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// Platform used in the experience of this learning activity.
        /// The "platform" property MUST only be used if the Statement's Object is an Activity.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Code representing the language in which the experience being recorded in this Statement (mainly) occurred in, if applicable and known.
        /// </summary>
        public string Language { get; set; }


        /// <summary>
        /// Another Statement to be considered as context for this Statement.
        /// </summary>
        public StatementRef Statement { get; set; }

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

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
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
