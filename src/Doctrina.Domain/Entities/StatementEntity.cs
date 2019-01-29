using System;

namespace Doctrina.Domain.Entities
{
    public class StatementEntity : StatementBaseEntity
    {
        public Guid StatementId { get; set; }

        public DateTimeOffset Stored { get; set; }

        public bool Voided { get; set; } = false;

        public string Version { get; set; }

        public Guid? AuthorityId { get; set; }

        public string FullStatement { get; set; }

        #region Navigation Properties
        public virtual AgentEntity Authority { get; set; }

        #endregion

        /// <summary>
        /// Gets the attached object
        /// </summary>
        public override IStatementObjectEntity Object
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
