using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    /// <summary>
    /// An Agent (an individual) is a persona or system.
    /// </summary>
    public class Agent : StatementObjectBase, IInvenseFunctionalIdenfitiers, IAgent, IStatementObject
    {
        protected override ObjectType OBJECT_TYPE => ObjectType.Agent;

        public Agent() : base() { }
        public Agent(string jsonString) : this((JsonString)jsonString) { }
        public Agent(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public Agent(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public Agent(JToken jobj, ApiVersion version) : base(jobj, version)
        {
            if (!AllowObject(jobj))
            {
                return;
            }

            if (DisallowNullValue(jobj["objectType"]) && AllowString(jobj["objectType"]))
            {
                Name = jobj.Value<string>("objectType");
            }

            if (DisallowNullValue(jobj["name"]) && AllowString(jobj["name"]))
            {
                Name = jobj.Value<string>("name");
            }

            if (DisallowNullValue(jobj["mbox"]) && AllowString(jobj["mbox"]))
            {
                Mbox = new Mbox(jobj.Value<string>("mbox"));
            }

            if (DisallowNullValue(jobj["mbox_sha1sum"]) && AllowString(jobj["mbox_sha1sum"]))
            {
                Mbox_SHA1SUM = jobj.Value<string>("mbox_sha1sum");
            }

            if (DisallowNullValue(jobj["openid"]) && AllowString(jobj["openid"]))
            {
                OpenId = new Iri(jobj.Value<string>("openid"));
            }

            if (DisallowNullValue(jobj["account"]) && AllowObject(jobj["account"]))
            {
                Account = new Account(jobj["account"], version);
            }
        }

        /// <summary>
        /// Agent. This property is optional except when the Agent is used as a Statement's object.
        /// </summary>
        public new ObjectType ObjectType { get { return OBJECT_TYPE; } }

        /// <summary>
        /// Full name of the Agent. (Optional)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The required format is "mailto:email address". 
        /// Only email addresses that have only ever been and will ever be assigned to this Agent, but no others, SHOULD be used for this property and mbox_sha1sum.
        /// </summary>
        public Mbox Mbox { get; set; }

        /// <summary>
        /// The hex-encoded SHA1 hash of a mailto IRI (i.e. the value of an mbox property). An LRS MAY include Agents with a matching hash when a request is based on an mbox.
        /// </summary>
        public string Mbox_SHA1SUM { get; set; }

        /// <summary>
        /// An openID that uniquely identifies the Agent.
        /// </summary>
        public Iri OpenId { get; set; }

        /// <summary>
        /// A user account on an existing system e.g. an LMS or intranet.
        /// </summary>
        public Account Account { get; set; }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = base.ToJToken(version, format);

            if(Name != null)
            {
                jobj["name"] = Name;
            }

            if(Mbox != null)
            {
                jobj["mbox"] = Mbox.ToString();
            }

            if(Mbox_SHA1SUM != null)
            {
                jobj["mbox_sha1sum"] = Mbox_SHA1SUM;
            }

            if(OpenId != null)
            {
                jobj["openid"] = OpenId.ToString();
            }

            if(Account != null)
            {
                jobj["account"] = Account.ToJToken(version, format);
            }

            return jobj;
        }

        public bool IsAnonymous()
        {
            return (Mbox == null
                && string.IsNullOrEmpty(Mbox_SHA1SUM)
                && Account == null
                && OpenId == null);
        }

        public bool IsIdentified()
        {
            return !IsAnonymous();
        }

        public List<string> GetIdentifiersByName()
        {
            var ids = new List<string>();
            if (Mbox != null)
            {
                ids.Add(nameof(Mbox).ToLower());
            }

            if (!string.IsNullOrEmpty(Mbox_SHA1SUM))
            {
                ids.Add(nameof(Mbox_SHA1SUM));
            }

            if (Account != null)
            {
                ids.Add(nameof(Account));
            }

            if (OpenId != null)
            {
                ids.Add(nameof(OpenId));
            }

            return ids;
        }

        public override bool Equals(object obj)
        {
            var agent = obj as Agent;
            return agent != null &&
                   base.Equals(obj) &&
                   ObjectType == agent.ObjectType &&
                   Name == agent.Name &&
                   EqualityComparer<Mbox>.Default.Equals(Mbox, agent.Mbox) &&
                   Mbox_SHA1SUM == agent.Mbox_SHA1SUM &&
                   EqualityComparer<Iri>.Default.Equals(OpenId, agent.OpenId) &&
                   EqualityComparer<Account>.Default.Equals(Account, agent.Account);
        }

        public override int GetHashCode()
        {
            var hashCode = -790879124;
            hashCode = hashCode * -1521134295 + ObjectType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Mbox>.Default.GetHashCode(Mbox);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Mbox_SHA1SUM);
            hashCode = hashCode * -1521134295 + EqualityComparer<Iri>.Default.GetHashCode(OpenId);
            hashCode = hashCode * -1521134295 + EqualityComparer<Account>.Default.GetHashCode(Account);
            return hashCode;
        }

        public static bool operator ==(Agent agent1, Agent agent2)
        {
            return EqualityComparer<Agent>.Default.Equals(agent1, agent2);
        }

        public static bool operator !=(Agent agent1, Agent agent2)
        {
            return !(agent1 == agent2);
        }
    }
}
