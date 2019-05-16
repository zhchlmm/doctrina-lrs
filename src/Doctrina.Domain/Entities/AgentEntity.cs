using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Doctrina.Domain.Entities
{
    public class AgentEntity : ObjectBase, IStatementObjectEntity
    {
        /// <summary>
        /// MD5 hash of agent identifier
        /// </summary>
        public string AgentHash { get; set; }

        public EntityObjectType ObjectType { get; set; }

        public string Name { get; set; }

        public string Mbox { get; set; }

        public string Mbox_SHA1SUM { get; set; }

        public string OpenId { get; set; }

        public Account Account { get; set; }

        public string ComputeHash()
        {
            using (var md5 = MD5.Create())
            {
                string key = GetInverseFunctionalIndentifier();
                byte[] bytes = Encoding.UTF8.GetBytes(key);
                byte[] hash = md5.ComputeHash(bytes);
                return Encoding.UTF8.GetString(hash);
            }
        }

        public string GetInverseFunctionalIndentifier()
        {
            return GetInverseFunctionalIdentifiers().FirstOrDefault();
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

            if (Account != null)
            {
                var uriBuilder = new UriBuilder(Account.HomePage);
                uriBuilder.UserName = Account.Name;
                ids.Add(uriBuilder.ToString());
            }

            if (ids.Count == 0 && ObjectType == EntityObjectType.Group)
            {
                // Is anonymous group, generate unique id
                ids.Add(Guid.NewGuid().ToString());
            }

            return ids;
        }
    }
}
