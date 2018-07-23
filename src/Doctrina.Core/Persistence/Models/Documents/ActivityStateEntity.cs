using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Doctrina.Core.Persistence.Models;

namespace Doctrina.Core.Persistence.Models
{
    public class ActivityStateEntity : IQueryableAgent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ActivityStateId { get; set; }

        [StringLength(Constants.MAX_URL_LENGTH)]
        public string StateId { get; set; }

        [StringLength(Constants.MAX_URL_LENGTH)]
        public string ActivityId { get; set; }

        public Guid? RegistrationId { get; set; }

        public Guid AgentId { get; set; }

        public Guid DocumentId { get; set; }

        #region Navigation Properties
        [ForeignKey("agentId")]
        public virtual AgentEntity Agent { get; set; }

        [ForeignKey("activityId")]
        public virtual ActivityEntity Activity { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public virtual DocumentEntity Document { get; set; }
        #endregion
    }
}
