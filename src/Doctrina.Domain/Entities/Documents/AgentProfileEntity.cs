using System;

namespace Doctrina.Domain.Entities.Documents
{
    public class AgentProfileEntity : IQueryableAgent, IAgentProfileEntity
    {
        public Guid AgentProfileId { get; set; }

        public string ProfileId { get; set; }

        public DateTime Updated { get; set; }

        public string ContentType { get; set; }

        public string ETag { get; set; }

        public string AgentHash { get; set; }

        public virtual AgentEntity Agent { get; set; }

        public virtual DocumentEntity Document { get; set; }
    }
}
