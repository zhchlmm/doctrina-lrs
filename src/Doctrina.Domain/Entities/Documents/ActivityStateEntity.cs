using System;

namespace Doctrina.Domain.Entities.Documents
{
    public class ActivityStateEntity
    {
        public string StateId { get; set; }

        public Guid? Registration { get; set; }

        public AgentEntity Agent { get; set; }

        public ActivityEntity Activity { get; set; }

        public DocumentEntity Document { get; set; }
    }
}
