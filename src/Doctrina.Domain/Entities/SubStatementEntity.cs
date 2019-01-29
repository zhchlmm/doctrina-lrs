using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Domain.Entities
{
    public class SubStatementEntity : StatementBaseEntity, IStatementObjectEntity
    {
        public Guid SubStatementId { get; set; }

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
                        // TODO: StatementRef is nullable
                    //case EntityObjectType.StatementRef:
                    //    return ObjectStatementRef;
                }

                return null;
            }
        }
    }
}
