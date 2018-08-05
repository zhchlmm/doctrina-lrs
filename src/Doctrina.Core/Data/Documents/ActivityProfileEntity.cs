using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data.Documents
{
    public class ActivityProfileEntity : IActivityProfileEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Key { get; set; }

        [Required]
        [StringLength(Constants.MAX_URL_LENGTH)]
        public string ProfileId { get; set; }

        public DateTime UpdateDate { get; set; }

        [Required]
        [StringLength(Constants.MAX_URL_LENGTH)]
        public Guid ActivityKey { get; set; }

        public Guid? RegistrationId { get; set; }

        [Required]
        public Guid AgentId { get; set; }

        [Required]
        public Guid DocumentId { get; set; }

        #region Navigation Properties
        [ForeignKey(nameof(AgentId))]
        public virtual AgentEntity Agent { get; set; }

        [ForeignKey(nameof(ActivityKey))]
        public virtual ActivityEntity Activity { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public virtual DocumentEntity Document { get; set; }
        #endregion
    }
}
