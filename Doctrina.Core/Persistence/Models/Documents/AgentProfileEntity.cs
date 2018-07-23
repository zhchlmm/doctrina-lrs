using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Doctrina.Core.Persistence.Models;

namespace Doctrina.Core.Persistence.Models
{
    public class AgentProfileEntity : IQueryableAgent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Key { get; set; }

        [StringLength(Constants.MAX_URL_LENGTH)]
        public string ProfileId { get; set; }

        public DateTime Updated { get; set; }

        [StringLength(255)]
        public string ContentType { get; set; }

        [StringLength(50)]
        public string ETag { get; set; }

        public Guid AgentId { get; set; }

        [ForeignKey(nameof(AgentId))]
        public virtual AgentEntity Agent {get;set;}

        public Guid DocumentId { get; set; }
        public virtual DocumentEntity Document { get; set; }
    }
}
