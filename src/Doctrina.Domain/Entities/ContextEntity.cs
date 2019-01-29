using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Domain.Entities
{
    public class ContextEntity
    {
        public Guid ContextId { get; set; }

        public Guid? Registration { get; set; }

        public Guid? InstructorId { get; set; }

        public Guid? TeamId { get; set; }

        public string Revision { get; set; }

        public string Platform { get; set; }

        public string Language { get; set; }

        public string Extensions { get; set; }

        public Guid? StatementId { get; set; }

        public Guid? ContextActivitiesId { get; set; }

        #region Navigation properties
        public virtual AgentEntity Instructor { get; set; }

        public virtual AgentEntity Team { get; set; }

        public virtual ContextActivitiesEntity ContextActivities { get; set; }
        #endregion
    }
}
