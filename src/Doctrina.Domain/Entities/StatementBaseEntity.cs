using System;
using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class StatementBaseEntity
    {
        public StatementBaseEntity()
        {
            Attachments = new HashSet<AttachmentEntity>();
        }

        public EntityObjectType ObjectObjectType { get; set; }

        public Guid? ObjectAgentId { get; set; }

        public string ObjectActivityId { get; set; }


        public Guid? ObjectStatementRefId { get; set; }

        public Guid ActorId { get; set; }

        public string VerbId { get; set; }

        public Guid? ResultId { get; set; }

        public Guid? ContextId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        #region Navigation properties
        public virtual AgentEntity Actor { get; set; }

        public virtual AgentEntity ObjectAgent { get; set; }

        public virtual ActivityEntity ObjectActivity { get; set; }


        public virtual VerbEntity Verb { get; set; }

        public virtual ResultEntity Result { get; set; }

        public virtual ContextEntity Context { get; set; }

        public virtual ICollection<AttachmentEntity> Attachments { get; set; }
        #endregion

        public virtual IStatementObjectEntity Object
        {
            get
            {
                switch (ObjectObjectType)
                {
                    case EntityObjectType.Agent:
                    case EntityObjectType.Group:
                        return ObjectAgent;
                    case EntityObjectType.Activity:
                        return ObjectActivity;
                    //case EntityObjectType.SubStatement:
                    //    return ObjectSubStatement;
                    //case EntityObjectType.StatementRef:
                    //    return ObjectStatementRef;
                }

                return null;
            }
        }
    }
}
