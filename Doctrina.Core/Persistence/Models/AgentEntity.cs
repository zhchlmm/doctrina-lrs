using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Persistence.Models
{
    public class AgentEntity : IStatementObjectEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(),
            StringLength(6)]
        public EntityObjectType ObjectType { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Mbox { get; set; }

        [StringLength(40)]
        public string Mbox_SHA1SUM { get; set; }

        [StringLength(Constants.MAX_URL_LENGTH)]
        public string OpenId { get; set; }

        [StringLength(192)]
        public string OauthIdentifier { get; set; }

        [StringLength(Constants.MAX_URL_LENGTH)]
        public string Account_HomePage { get; set; }

        [StringLength(50)]
        public string Account_Name { get; set; }

        public virtual ICollection<GroupMemberEntity> Members { get; set; }
    }
}
