using Doctrina.Domain.Entities;
using System;

namespace Doctrina.Persistence.Repositories
{
    public interface IAgentGroupMemberRepository
    {
        GroupMemberEntity GetRelated(Guid groupId, Guid agentId);
    }
}