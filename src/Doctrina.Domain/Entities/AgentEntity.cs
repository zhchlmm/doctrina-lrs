using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Doctrina.Domain.Entities
{
    public class AgentEntity : IStatementObjectEntity
    {
        /// <summary>
        /// MD5 hash of agent identifier
        /// </summary>
        public string AgentEntityId { get; set; }

        public EntityObjectType ObjectType { get; set; }

        public string Name { get; set; }

        public string Mbox { get; set; }

        public string Mbox_SHA1SUM { get; set; }

        public string OpenId { get; set; }

        //public string OauthIdentifier { get; set; }

        public Account Account { get; set; }

        public string GenerateIdentifierHash()
        {
            ICollection<string> ids = GetInverseFunctionalIdentifiers();

            ids.Add(Enum.GetName(typeof(EntityObjectType), ObjectType));

            using (var md5 = MD5.Create())
            {
                string key = string.Join("|", ids);
                byte[] bytes = Encoding.UTF8.GetBytes(key);
                byte[] hash = md5.ComputeHash(bytes);
                return Encoding.UTF8.GetString(hash);
            }
        }

        public ICollection<string> GetInverseFunctionalIdentifiers()
        {
            HashSet<string> ids = new HashSet<string>();

            if (!string.IsNullOrWhiteSpace(Mbox))
            {
                ids.Add(Mbox);
            }

            if (!string.IsNullOrWhiteSpace(Mbox_SHA1SUM))
            {
                ids.Add(Mbox_SHA1SUM);
            }

            if (!string.IsNullOrWhiteSpace(OpenId))
            {
                ids.Add(OpenId);
            }

            if (!string.IsNullOrWhiteSpace(OauthIdentifier))
            {
                ids.Add(OauthIdentifier);
            }

            if (Account != null)
            {
                ids.Add(Account.HomePage + "|" + Account.Name);
            }

            // Is anonymous group?
            if (ids.Count == 0 && ObjectType == EntityObjectType.Group)
            {
                ids.Add(Guid.NewGuid().ToString());
            }

            return ids;
        }
    }
}
