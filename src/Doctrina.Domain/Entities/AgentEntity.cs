using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Domain.Entities
{
    public class AgentEntity : IStatementObjectEntity
    {
        public Guid AgentId { get; set; }

        public EntityObjectType ObjectType { get; set; }

        public string Name { get; set; }

        public string Mbox { get; set; }

        public string Mbox_SHA1SUM { get; set; }

        public string OpenId { get; set; }

        public string OauthIdentifier { get; set; }

        public Guid AccountId { get; set; }

        public virtual AccountEntity Account { get; set; }
    }
}
