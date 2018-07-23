using System;
using Doctrina.Core.Persistence.Models;

namespace Doctrina.Core.Repositories
{
    public interface IAgentGroupMemberRepository
    {
        GroupMemberEntity GetRelated(Guid groupId, Guid agentId);
    }
}