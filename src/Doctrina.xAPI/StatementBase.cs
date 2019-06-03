using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    public abstract class StatementBase : JsonModel<JToken>, IStatementBase, IAttachmentByHash
    {
        public StatementBase() : base(null, null) { }

        public StatementBase(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public StatementBase(JToken jobj, ApiVersion version) : base(jobj, version)
        {
            if (DisallowNullValue(jobj["actor"]))
            {
                var actor = jobj["actor"];
                if (actor["objectType"] != null && (string)actor["objectType"] == ObjectType.Group)
                {
                    Actor = new Group(actor, version);
                }
                else
                {
                    Actor = new Agent(actor, version);
                }
            }

            if (DisallowNullValue(jobj["verb"]))
            {
                Verb = new Verb(jobj["verb"], version);
            }

            var jObject = jobj["object"];
            if (DisallowNullValue(jObject) && AllowObject(jObject))
            {
                if (jObject["objectType"] != null)
                {
                    ObjectType objectType = jObject.Value<string>("objectType");
                    if(objectType != null)
                    {
                        Object = objectType.CreateInstance(jObject, version);
                    }
                    else
                    {
                        ParsingErrors.Add(jObject.Path, $"Is not a valid objectType.");
                    }
                }
                else if(jObject["id"] != null)
                {
                    // Assume activity
                    Object = ObjectType.Activity.CreateInstance(jObject, version);
                }
            }

            if (jobj["result"] != null)
            {
                Result = new Result(jobj["result"], version);
            }

            if (jobj["context"] != null)
            {
                Context = new Context(jobj["context"], version);
            }

            if (jobj["timestamp"] != null)
            {
                Timestamp = DateTimeOffset.Parse(jobj.Value<string>("timestamp"));
            }

            if (jobj["attachments"] != null)
            {
                Attachments = new AttachmentCollection(jobj["attachments"], version);
            }
        }

        /// <summary>
        /// Whom the Statement is about, as an Agent or Group Object.
        /// </summary>
        public Agent Actor { get; set; }

        /// <summary>
        /// Action taken by the Actor.
        /// </summary>
        public Verb Verb { get; set; }

        /// <summary>
        /// Activity, Agent, or another Statement that is the Object of the Statement.
        /// </summary>
        public IStatementObject Object { get; set; }

        /// <summary>
        /// Result Object, further details representing a measured outcome.
        /// </summary>
        public Result Result { get; set; }

        /// <summary>
        /// Context that gives the Statement more meaning. Examples: a team the Actor is working with, altitude at which a scenario was attempted in a flight simulator.
        /// </summary>
        public Context Context { get; set; }

        /// <summary>
        /// Timestamp of when the events described within this Statement occurred. Set by the LRS if not provided.
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

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

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
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

            if (Attachments != null && Attachments.Count > 0)
            {
                jobj["attachments"] = Attachments.ToJToken(version, format);
            }

            return jobj;
        }

        public Attachment GetAttachmentByHash(string sha2)
        {
            if (Attachments == null || Attachments.Count <= 0)
            {
                return null;
            }

            foreach (var attachment in Attachments)
            {
                if (attachment.SHA2 == sha2)
                {
                    return attachment;
                }
            }

            return null;
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
