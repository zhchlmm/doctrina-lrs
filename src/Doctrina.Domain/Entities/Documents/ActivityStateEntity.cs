using System;

namespace Doctrina.Domain.Entities.Documents
{
    public class ActivityStateEntity : IQueryableAgent
    {
        public string StateId { get; set; }

        public string ActivityId { get; set; }

        public Guid AgentId { get; set; }

        public Guid? RegistrationId { get; set; }

        #region Navigation Properties
        public virtual AgentEntity Agent { get; set; }

        public virtual ActivityEntity Activity { get; set; }

        public virtual DocumentEntity Document { get; set; }
        #endregion
    }
}
