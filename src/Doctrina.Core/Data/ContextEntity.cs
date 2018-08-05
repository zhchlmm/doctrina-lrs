using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data
{
    public class ContextEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ContextId { get; set; }

        public Guid? Registration { get; set; }

        public Guid? InstructorId { get; set; }

        public Guid? TeamId { get; set; }

        public string Revision { get; set; }

        public string Platform { get; set; }

        public string Language { get; set; }

        public string Extensions { get; set; }

        public Guid? StatementId { get; set; }

        public Guid ContextActivitiesId { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public virtual AgentEntity Instructor { get; set; }

        [ForeignKey(nameof(TeamId))]
        public virtual AgentEntity Team { get; set; }

        [ForeignKey(nameof(ContextActivitiesId))]
        public virtual ContextActivitiesEntity ContextActivities { get; set; }
    }
}
