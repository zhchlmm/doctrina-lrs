using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data
{
    public class ActivityEntity : IStatementObjectEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Key { get; set; }

        [Required]
        public string ActivityId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string CanonicalData { get; set; }

        public Guid? AuthorityId {get;set;}
        public virtual AgentEntity Authority { get; set; }

        [NotMapped]
        public EntityObjectType ObjectType { get; set; } = EntityObjectType.Activity;
    }
}
