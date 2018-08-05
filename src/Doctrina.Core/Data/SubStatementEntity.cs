using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data
{
    public class SubStatementEntity : IStatementObjectEntity, IStatementEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid ActorKey { get; set; }

        [Required]
        public Guid VerbKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the object attached
        /// </summary>
        [Required]
        public EntityObjectType ObjectType { get; set; }

        public Guid? ObjectAgentKey { get; set; }

        public Guid? ObjectActivityKey { get; set; }

        public Guid? ObjectStatementRefId { get; set; }

        public Guid? ResultId { get; set; }

        public DateTime? Timestamp { get; set; }

        public Guid? ContextId { get; set; }

        #region Navigation Properties
        [ForeignKey(nameof(ActorKey))]
        public virtual AgentEntity Actor { get; set; }

        [ForeignKey(nameof(ObjectAgentKey))]
        public virtual AgentEntity ObjectAgent { get; set; }

        [ForeignKey(nameof(ObjectActivityKey))]
        public virtual ActivityEntity ObjectActivity { get; set; }

        [ForeignKey(nameof(ObjectStatementRefId))]
        public virtual StatementEntity ObjectStatementRef { get; set; }

        [ForeignKey(nameof(VerbKey))]
        public virtual VerbEntity Verb { get; set; }

        [ForeignKey(nameof(ResultId))]
        public virtual ResultEntity Result { get; set; }

        [ForeignKey(nameof(ContextId))]
        public virtual ContextEntity Context { get; set; }

        /// <summary>
        /// Gets the attached object
        /// </summary>
        public IStatementObjectEntity Object
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
                    case EntityObjectType.StatementRef:
                        return ObjectStatementRef;
                }

                return null;
            }
        }
        #endregion
    }
}
