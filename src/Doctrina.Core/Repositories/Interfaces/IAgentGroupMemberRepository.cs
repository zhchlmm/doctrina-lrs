using System;
using Doctrina.Core.Data;

namespace Doctrina.Core.Repositories
{
    public interface IAgentGroupMemberRepository
    {
        GroupMemberEntity GetRelated(Guid groupId, Guid agentId);
    }
}