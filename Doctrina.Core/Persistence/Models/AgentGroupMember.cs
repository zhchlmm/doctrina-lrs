using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctrina.Core.Persistence;

namespace Doctrina.Core.Persistence.Models
{
    public class GroupMemberEntity
    {
        [ForeignKey(nameof(AgentEntity.Id))]
        public Guid GroupId { get; set; }

        [ForeignKey(nameof(AgentEntity.Id))]
        public Guid MemberId { get; set; }
    }
}
