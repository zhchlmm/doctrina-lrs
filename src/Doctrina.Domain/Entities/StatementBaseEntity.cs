using System;
using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class StatementBaseEntity
    {
        public EntityObjectType ObjectType { get; set; }

        public Guid? ObjectAgentKey { get; set; }

        public Guid? ObjectActivityKey { get; set; }

        public Guid? ObjectSubStatementId { get; set; }

        public Guid? ObjectStatementRefId { get; set; }

        public Guid ActorKey { get; set; }

        public Guid VerbKey { get; set; }

        public Guid? ResultId { get; set; }

        public Guid? ContextId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        #region Navigation properties
        public virtual AgentEntity Actor { get; set; }

        public virtual AgentEntity ObjectAgent { get; set; }

        public virtual ActivityEntity ObjectActivity { get; set; }

        public virtual SubStatementEntity ObjectSubStatement { get; set; }

        //public virtual StatementEntity ObjectStatementRef { get; set; }

        public virtual VerbEntity Verb { get; set; }

        public virtual ResultEntity Result { get; set; }

        public virtual ContextEntity Context { get; set; }

        public virtual ICollection<AttachmentEntity> Attachments { get; set; }
        #endregion

        public virtual IStatementObjectEntity Object
        {
            get
            {
                switch (ObjectType)
                {
                    case EntityObjectType.Agent:
                    case EntityObjectType.Group:
                        return ObjectAgent;
                    case EntityObjectType.Activity:
                        return ObjectActivity;
                    case EntityObjectType.SubStatement:
                        return ObjectSubStatement;
                        //case EntityObjectType.StatementRef:
                        //    return ObjectStatementRef;
                }

                return null;
            }
        }
    }
}
