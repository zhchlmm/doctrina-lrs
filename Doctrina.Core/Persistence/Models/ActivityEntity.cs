using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Persistence.Models
{
    public class ActivityEntity : IStatementObjectEntity
    {
        [Column("activityId"),
            Key,
            StringLength(Constants.MAX_URL_LENGTH)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ActivityId { get; set; }

        [Column("canonicalData", TypeName = "nvarchar(max)")]
        public string CanonicalData { get; set; }

        [Column("authorityId")]
        public Guid? AuthorityId {get;set;}
        public virtual AgentEntity Authority { get; set; }

        [NotMapped]
        public EntityObjectType ObjectType { get; set; } = EntityObjectType.Activity;
    }
}
