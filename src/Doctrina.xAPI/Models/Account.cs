using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Models
{
    /// <summary>
    /// A user account on an existing system, such as a private system (LMS or intranet) or a public system (social networking site).
    /// </summary>
    public class Account : JsonModel
    {
        /// <summary>
        /// The canonical home page for the system the account is on. This is based on FOAF's accountServiceHomePage.
        /// </summary>
        [JsonProperty("homePage",
            Required = Required.Always)]
        public IRI HomePage { get; set; }

        /// <summary>
        /// The unique id or name used to log in to this account. This is based on FOAF's accountName.
        /// </summary>
        [JsonProperty("name",
            Required = Required.Always)]
        public string Name { get; set; }
    }

    public class AccountSchema : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var schema = context.Generator.Generate(typeof(Account));
            return schema;
        }
    }
}
