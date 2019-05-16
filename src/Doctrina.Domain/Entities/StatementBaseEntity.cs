using System;
using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public abstract class StatementBaseEntity
    {
        public StatementBaseEntity()
        {
            Attachments = new HashSet<AttachmentEntity>();
        }

        public EntityObjectType ObjectObjectType { get; set; }
        public AgentEntity Actor { get; set; }
        public VerbEntity Verb { get; set; }

        public Guid? ObjectStatementRefId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public AgentEntity ObjectAgent { get; set; }

        public ActivityEntity ObjectActivity { get; set; }

        public ResultEntity Result { get; set; }

        public ContextEntity Context { get; set; }

        public virtual ICollection<AttachmentEntity> Attachments { get; set; }

        /// <summary>
        /// Gets the object of the current statement
        /// </summary>
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
