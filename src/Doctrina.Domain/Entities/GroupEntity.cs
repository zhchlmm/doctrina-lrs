using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class GroupEntity : AgentEntity
    {
        public GroupEntity()
        {
            Members = new HashSet<GroupMemberEntity>();
        }

        public virtual ICollection<GroupMemberEntity> Members { get; set; }
    }
}
