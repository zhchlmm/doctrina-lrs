using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctrina.Core;
using Doctrina.Core.Persistence;
using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Persistence.Models;

namespace Doctrina.Core.Persistence.Models
{
    public class ActivityProfileEntity : IQueryableAgent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Key { get; set; }

        [StringLength(Constants.MAX_URL_LENGTH)]
        public string ProfileId { get; set; }

        public DateTime UpdateDate { get; set; }

        [StringLength(Constants.MAX_URL_LENGTH)]
        public string ActivityId { get; set; }

        public Guid? RegistrationId { get; set; }

        public Guid AgentId { get; set; }

        public Guid DocumentId { get; set; }

        #region Navigation Properties
        [ForeignKey(nameof(AgentId))]
        public virtual AgentEntity Agent { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public virtual ActivityEntity Activity { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public virtual DocumentEntity Document { get; set; }
        #endregion
    }
}
