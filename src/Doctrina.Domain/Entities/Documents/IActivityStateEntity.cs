using System;

namespace Doctrina.Domain.Entities.Documents
{
    public interface IActivityStateEntity
    {
        ActivityEntity Activity { get; set; }
        string ActivityHash { get; set; }
        AgentEntity Agent { get; set; }
        string AgentHash { get; set; }
        Guid? Registration { get; set; }
        string StateId { get; set; }
    }
}