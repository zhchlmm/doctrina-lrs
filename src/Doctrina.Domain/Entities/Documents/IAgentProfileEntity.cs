using System;

namespace Doctrina.Domain.Entities.Documents
{
    public interface IAgentProfileEntity
    {
        AgentEntity Agent { get; set; }
        string AgentHash { get; set; }
        string ContentType { get; set; }
        DocumentEntity Document { get; set; }
        //Guid DocumentId { get; set; }
        string ETag { get; set; }
        Guid AgentProfileId { get; set; }
        string ProfileId { get; set; }
        DateTime Updated { get; set; }
    }
}