using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Models
{
    /// <summary>
    /// The Person Object is very similar to an Agent Object, but instead of each attribute having a single value, each attribute has an array value, and it is legal to include multiple identifying properties. This is different from the FOAF concept of person, person is being used here to indicate a person-centric view of the LRS Agent data, but Agents just refer to one persona (a person in one context).
    /// </summary>
    [JsonObject]
    public class Person
    {
        public Person()
        {
            Name = new List<string>();
            Mbox = new List<Mbox>();
            Mbox_sha1sum = new List<string>();
            OpenId = new List<Iri>();
            Account = new List<Account>();
        }

        [JsonProperty("objectType")]
        public string ObjectType { get; private set; } = "Person";

        [JsonProperty("name")]
        public List<string> Name { get; set; }

        [JsonProperty("mbox")]
        public List<Mbox> Mbox { get; set; }

        [JsonProperty("mbox_sha1sum")]
        public List<string> Mbox_sha1sum { get; set; }

        [JsonProperty("openid")]
        public List<Iri> OpenId { get; set; }

        [JsonProperty("account")]
        public List<Account> Account { get; set; }

        /// <summary>
        /// Adds Agent to Person object
        /// </summary>
        /// <param name="agent">Agent Object with a single identifier, It is not a Person Object, nor is it a Group.</param>
        public void Add(Agent agent)
        {
            if (agent == null)
                throw new ArgumentNullException("agent");

            if (agent.ObjectType == Models.ObjectType.Group)
                throw new ArgumentException("Groups are not allowed within an Person Object.");

            if(agent.Account != null)
            {
                Account.Add(agent.Account);
                return;
            }

            if (agent.Mbox != null)
            {
                Mbox.Add(agent.Mbox);
                return;
            }

            if (!string.IsNullOrWhiteSpace(agent.MboxSHA1SUM))
            {
                Mbox_sha1sum.Add(agent.MboxSHA1SUM);
                return;
            }

            if(agent.OpenId != null)
            {
                OpenId.Add(agent.OpenId);
                return;
            }
        }

        public void Combine(Agent agent)
        {
            Add(agent);
        }
    }
}
