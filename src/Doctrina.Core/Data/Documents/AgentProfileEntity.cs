using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data.Documents
{
    public class AgentProfileEntity : IQueryableAgent, IAgentProfileEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Key { get; set; }

        [Required]
        [StringLength(Constants.MAX_URL_LENGTH)]
        public string ProfileId { get; set; }

        public DateTime Updated { get; set; }

        [Required]
        [StringLength(255)]
        public string ContentType { get; set; }

        [Required]
        [StringLength(50)]
        public string ETag { get; set; }

        [Required]
        public Guid AgentId { get; set; }

        [Required]
        public Guid DocumentId { get; set; }

        [ForeignKey(nameof(AgentId))]
        public virtual AgentEntity Agent {get;set;}

        [ForeignKey(nameof(DocumentId))]
        public virtual DocumentEntity Document { get; set; }
    }
}
