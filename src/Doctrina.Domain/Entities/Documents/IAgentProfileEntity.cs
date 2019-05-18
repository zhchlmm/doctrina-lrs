using System;

namespace Doctrina.Domain.Entities.Documents
{
    public interface IAgentProfileEntity
    {
        string ProfileId { get; set; }
        AgentEntity Agent { get; set; }
        string AgentHash { get; set; }
    }
}