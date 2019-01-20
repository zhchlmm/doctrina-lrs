using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
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
        public Uri HomePage { get; set; }

        /// <summary>
        /// The unique id or name used to log in to this account. This is based on FOAF's accountName.
        /// </summary>
        [JsonProperty("name",
            Required = Required.Always)]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var account = obj as Account;
            return account != null &&
                   base.Equals(obj) &&
                   EqualityComparer<Uri>.Default.Equals(HomePage, account.HomePage) &&
                   Name == account.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = 74222723;
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(HomePage);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public static bool operator ==(Account account1, Account account2)
        {
            return EqualityComparer<Account>.Default.Equals(account1, account2);
        }

        public static bool operator !=(Account account1, Account account2)
        {
            return !(account1 == account2);
        }
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
