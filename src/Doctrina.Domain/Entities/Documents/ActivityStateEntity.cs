using System;

namespace Doctrina.Domain.Entities.Documents
{
    public class ActivityStateEntity : DocumentBaseEntity, IDocumentEntity, IActivityStateEntity
    {
        public ActivityStateEntity()
        {
        }

        public ActivityStateEntity(byte[] content, string contentType) : base(content, contentType)
        {
        }

        public string StateId { get; set; }
        public Guid? Registration { get; set; }
        public string AgentHash { get; set; }
        public string ActivityHash { get; set; }

        public virtual AgentEntity Agent { get; set; }
        public virtual ActivityEntity Activity { get; set; }
    }
}
