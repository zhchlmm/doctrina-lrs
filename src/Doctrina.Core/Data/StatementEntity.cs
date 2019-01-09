using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Core.Data
{
    public class StatementEntity : IStatementEntityBase, IStatementObjectEntity
    {
        [Column, Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StatementId { get; set; }

        /// <summary>
        /// Gets or sets the type of the object attached
        /// </summary>
        [NotMapped]
        [StringLength(Constants.OBJECT_TYPE_LENGTH)]
        public EntityObjectType ObjectType { get; set; }

        public Guid? ObjectAgentKey { get; set; }

        public Guid? ObjectActivityKey { get; set; }

        public Guid? ObjectSubStatementId { get; set; }

        public Guid? ObjectStatementRefId { get; set; }

        [Required]
        public Guid ActorKey { get; set; }

        [Required]
        public Guid VerbKey { get; set; }

        public Guid? ResultId { get; set; }

        public Guid? ContextId { get; set; }

        [Required]
        public DateTimeOffset Timestamp { get; set; }

        [Required]
        public DateTimeOffset Stored { get; set; }

        public bool Voided { get; set; } = false;

        [StringLength(7)]
        public string Version { get; set; }

        public Guid? User { get; set; }

        public Guid? AuthorityId { get; set; }

        [Column(TypeName = "ntext")]
        public string FullStatement { get; set; }

        #region Navigation Properties
        [ForeignKey(nameof(ActorKey))]
        public virtual AgentEntity Actor { get; set; }

        [ForeignKey(nameof(ObjectAgentKey))]
        public virtual AgentEntity ObjectAgent { get; set; }

        [ForeignKey(nameof(ObjectActivityKey))]
        public virtual ActivityEntity ObjectActivity { get; set; }

        [ForeignKey(nameof(ObjectSubStatementId))]
        public virtual SubStatementEntity ObjectSubStatement { get; set; }

        // Might not exist yet
        //[ForeignKey(nameof(ObjectStatementRefId))]
        //public virtual StatementEntity ObjectStatementRef { get; set; }

        [ForeignKey(nameof(VerbKey))]
        public virtual VerbEntity Verb { get; set; }

        [ForeignKey(nameof(ResultId))]
        public virtual ResultEntity Result { get; set; }

        [ForeignKey(nameof(ContextId))]
        public virtual ContextEntity Context { get; set; }

        [ForeignKey(nameof(AuthorityId))]
        public virtual AgentEntity Authority { get; set; }

        public virtual List<AttachmentEntity> Attachments { get; set; }
        #endregion

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
