using System;
using Doctrina.Persistence.Entities;

namespace Doctrina.Persistence.Repositories
{
    public interface IAgentGroupMemberRepository
    {
        GroupMemberEntity GetRelated(Guid groupId, Guid agentId);
    }
}