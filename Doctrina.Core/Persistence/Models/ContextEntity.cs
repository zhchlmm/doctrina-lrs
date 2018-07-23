using Doctrina.Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core.Persistence.Models
{
    public class ContextEntity
    {
        [Column(),
            Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ContextId { get; set; }

        [Column()]
        public Guid? Registration { get; set; }

        [Column()]
        public Guid? InstructorId { get; set; }

        [Column()]
        public Guid? TeamId { get; set; }

        [Column()]
        public string Revision { get; set; }

        [Column()]
        public string Platform { get; set; }

        [Column()]
        public string Language { get; set; }

        [Column()]
        public string Extensions { get; set; }

        [Column()]
        public Guid? StatementId { get; set; }

        public Guid ContextActivitiesId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public virtual AgentEntity Instructor { get; set; }

        [ForeignKey(nameof(TeamId))]
        public virtual AgentEntity Team { get; set; }

        [ForeignKey(nameof(StatementId))]
        public virtual StatementEntity Statement { get; set; }

        [ForeignKey(nameof(ContextActivitiesId))]
        public virtual ContextActivitiesEntity ContextActivities { get; set; }
    }
}
