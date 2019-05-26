using System;
using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class StatementEntity : IStatementBaseEntity
    {
        public StatementEntity()
        {
            Attachments = new HashSet<AttachmentEntity>();
        }

        public Guid? StatementId { get; set; }
        public virtual AgentEntity Actor { get; set; }
        public virtual VerbEntity Verb { get; set; }
        public virtual StatementObjectEntity Object { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public virtual ResultEntity Result { get; set; }
        public virtual ContextEntity Context { get; set; }
        public virtual ICollection<AttachmentEntity> Attachments { get; set; }

        public DateTimeOffset? Stored { get; set; }
        public string Version { get; set; }
        public Guid? AuthorityId { get; set; }
        public string FullStatement { get; set; }
        public bool Voided { get; set; } = false;

        #region Navigation Properties
        public virtual AgentEntity Authority { get; set; }
        #endregion

    }
}
