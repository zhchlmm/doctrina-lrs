using System;

namespace Doctrina.Domain.Entities.Documents
{
    public class ActivityStateEntity : IQueryableAgent
    {
        public string StateId { get; set; }

        public string ActivityEntityId { get; set; }

        public string AgentEntityId { get; set; }

        public Guid? Registration { get; set; }

        #region Navigation Properties
        public virtual AgentEntity Agent { get; set; }

        public virtual ActivityEntity Activity { get; set; }

        public virtual DocumentEntity Document { get; set; }
        #endregion
    }
}
