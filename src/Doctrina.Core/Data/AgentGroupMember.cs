using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data
{
    public class GroupMemberEntity
    {
        [Key, 
            DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(AgentEntity.Key))]
        public Guid GroupId { get; set; }

        [ForeignKey(nameof(AgentEntity.Key))]
        public Guid MemberId { get; set; }
    }
}
