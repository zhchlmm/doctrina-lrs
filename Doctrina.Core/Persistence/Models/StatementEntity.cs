using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Core.Models;

namespace Doctrina.Core.Persistence.Models
{
    public class StatementEntity : IStatementBase, IStatementObjectEntity
    {
        [Column(),
            Key]
        public Guid StatementId { get; set; }

        /// <summary>
        /// Gets or sets the type of the object attached
        /// </summary>
        [Column(),
            StringLength(Constants.OBJECT_TYPE_LENGTH)]
        public EntityObjectType ObjectType { get; set; }

        [Column()]
        public Guid? ObjectAgentId { get; set; }

        [Column()]
        public string ObjectActivityId { get; set; }

        [Column()]
        public Guid? ObjectSubStatementId { get; set; }

        [Column()]
        public Guid? ObjectStatementRefId { get; set; }

        [Column]
        [Required]
        public Guid? ActorId { get; set; }

        [Column]
        [Required]
        public string VerbId { get; set; }

        public Guid? ResultId { get; set; }

        public Guid? ContextId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public DateTime Stored { get; set; }

        public bool Voided { get; set; } = false;

        [StringLength(7)]
        public string Version { get; set; }

        public Guid? User { get; set; }

        public Guid? AuthorityId { get; set; }

        [Column(TypeName = "ntext")]
        public string FullStatement { get; set; }

        #region Navigation Properties
        [ForeignKey(nameof(ActorId))]
        public virtual AgentEntity Actor { get; set; }

        [ForeignKey(nameof(ObjectAgentId))]
        public virtual AgentEntity ObjectAgent { get; set; }

        [ForeignKey(nameof(ObjectActivityId))]
        public virtual ActivityEntity ObjectActivity { get; set; }

        [ForeignKey(nameof(ObjectSubStatementId))]
        public virtual SubStatementEntity ObjectSubStatement { get; set; }

        [ForeignKey(nameof(ObjectStatementRefId))]
        public virtual StatementEntity ObjectStatementRef { get; set; }

        [ForeignKey(nameof(VerbId))]
        public virtual VerbEntity Verb { get; set; }

        [ForeignKey(nameof(ResultId))]
        public virtual ResultEntity Result { get; set; }

        [ForeignKey(nameof(ContextId))]
        public virtual ContextEntity Context { get; set; }

        [ForeignKey(nameof(AuthorityId))]
        public virtual AgentEntity Authority { get; set; }
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
                    case EntityObjectType.StatementRef:
                        return ObjectStatementRef;
                }

                return null;
            }
        }

    }
}
