using Doctrina.xAPI.Json.Converters;
using Doctrina.xAPI.Schema.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Doctrina.xAPI.Models
{
    [JsonObject]
    [JsonConverter(typeof(AgentJsonConverter))]
    [JSchemaGenerationProvider(typeof(AgentSchemaProvider))]
    public class Agent : StatementTargetBase, IInvenseFunctionalIdenfitier, IAgent
    {
        protected override ObjectType OBJECT_TYPE => ObjectType.Agent; 

        /// <summary>
        /// Agent. This property is optional except when the Agent is used as a Statement's object.
        /// </summary>
        [JsonProperty("objectType",
            Order = 1,
            Required = Required.DisallowNull)]
        [EnumDataType(typeof(ObjectType))]
        [JsonConverter(typeof(ObjectTypeConverter))]
        public new ObjectType ObjectType { get { return OBJECT_TYPE; } }

        /// <summary>
        /// Full name of the Agent. (Optional)
        /// </summary>
        [JsonProperty("name", 
            Order = 2,
            Required = Required.DisallowNull, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// The required format is "mailto:email address". 
        /// Only email addresses that have only ever been and will ever be assigned to this Agent, but no others, SHOULD be used for this property and mbox_sha1sum.
        /// </summary>
        [JsonProperty("mbox", 
            Order = 3,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public Mbox Mbox { get; set; }

        /// <summary>
        /// The hex-encoded SHA1 hash of a mailto IRI (i.e. the value of an mbox property). An LRS MAY include Agents with a matching hash when a request is based on an mbox.
        /// </summary>
        [JsonProperty("mbox_sha1sum", 
            Order = 4,
            Required = Required.DisallowNull, 
            NullValueHandling = NullValueHandling.Ignore)]
        public string MboxSHA1SUM { get; set; }

        /// <summary>
        /// An openID that uniquely identifies the Agent.
        /// </summary>
        [JsonProperty("openid", 
            Order = 5,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public Iri OpenId { get; set; }

        /// <summary>
        /// A user account on an existing system e.g. an LMS or intranet.
        /// </summary>
        [JsonProperty("account", 
            Order = 6,
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        public Account Account { get; set; }

        public static Agent Parse(string jsonAgent)
        {
            // This thing calls typeconveter
            //return JsonConvert.DeserializeObject<Agent>(jsonAgent);
            return JObject.Parse(jsonAgent).ToObject<Agent>();
        }

        public static bool TryParse(string value, out Agent agent)
        {
            agent = null;
            try
            {
                agent = JObject.Parse(value).ToObject<Agent>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsAnonymous()
        {
            return (Mbox == null
                && string.IsNullOrEmpty(MboxSHA1SUM)
                && Account == null
                && OpenId == null);
        }

        public bool IsIdentified()
        {
            return !IsAnonymous();
        }

        public List<string> GetIdentifiers()
        {
            var ids = new List<string>();
            if(Mbox != null)
            {
                ids.Add(nameof(Mbox).ToLower());
            }

            if (!string.IsNullOrEmpty(MboxSHA1SUM)){
                ids.Add(nameof(MboxSHA1SUM));
            }
            if(Account != null)
            {
                ids.Add(nameof(Account));
            }
            if(OpenId != null)
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
                   MboxSHA1SUM == agent.MboxSHA1SUM &&
                   EqualityComparer<Iri>.Default.Equals(OpenId, agent.OpenId) &&
                   EqualityComparer<Account>.Default.Equals(Account, agent.Account);
        }

        public override int GetHashCode()
        {
            var hashCode = -790879124;
            hashCode = hashCode * -1521134295 + ObjectType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Mbox>.Default.GetHashCode(Mbox);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MboxSHA1SUM);
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
