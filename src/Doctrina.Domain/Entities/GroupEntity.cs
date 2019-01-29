using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class GroupEntity : AgentEntity
    {
        public virtual ICollection<GroupMemberEntity> Members { get; set; }
    }
}
