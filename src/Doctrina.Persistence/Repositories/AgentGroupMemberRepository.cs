using Doctrina.Domain.Entities;
using System;
using System.Linq;

namespace Doctrina.Persistence.Repositories
{
    public class AgentGroupMemberRepository : IAgentGroupMemberRepository
    {
        private readonly DoctrinaDbContext context;

        public AgentGroupMemberRepository(DoctrinaDbContext context)
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
