using System;

namespace Doctrina.Domain.Entities.Documents
{
    public class AgentProfileEntity : DocumentBaseEntity, IAgentProfileEntity, IDocumentEntity
    {
        public AgentProfileEntity()
        {
        }

        public AgentProfileEntity(byte[] content, string contentType) : base(content, contentType)
        {
        }

        public string ProfileId { get; set; }

        public string AgentHash { get; set; }

        public virtual AgentEntity Agent { get; set; }
    }
}
