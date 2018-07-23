using xAPI.Core.Converters;
using xAPI.Core.Schema.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace xAPI.Core.Models
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
        public IRI OpenId { get; set; }

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

        public PropertyInfo GetIdentifier()
        {
            if(Mbox != null)
            {
                return this.GetType().GetProperty(nameof(Mbox));
            }

            if (!string.IsNullOrWhiteSpace(MboxSHA1SUM))
            {
                return this.GetType().GetProperty(nameof(MboxSHA1SUM));
            }

            if(Account != null)
            {
                return this.GetType().GetProperty(nameof(Account));
            }

            if(OpenId != null)
            {
                return this.GetType().GetProperty(nameof(OpenId));
            }

            return null;
        }
    }
}
