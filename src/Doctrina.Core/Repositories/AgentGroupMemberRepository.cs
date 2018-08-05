using Doctrina.Core;
using Doctrina.Core.Data;
using System;
using System.Linq;

namespace Doctrina.Core.Repositories
{
    public class AgentGroupMemberRepository : IAgentGroupMemberRepository
    {
        private readonly DoctrinaContext context;

        public AgentGroupMemberRepository(DoctrinaContext context)
        {
            this.context = context;
        }

        public GroupMemberEntity GetRelated(Guid groupId, Guid memberId)
        {
            var result = this.context.GroupMembers.Where(x => x.GroupId == groupId && x.MemberId == memberId).ToList();

            if(result != null && result.Any())
            {
                return result.FirstOrDefault();
            }

            return null;
        }

        internal void Insert(GroupMemberEntity related)
        {
            throw new NotImplementedException();
        }
    }
}
