using System;

namespace Doctrina.Domain.Entities
{
    public class ActivityEntity : IStatementObjectEntity
    {
        /// <summary>
        /// SHA-1 of <see cref="Id"/>
        /// </summary>
        public string ActivityId { get; set; }

        public EntityObjectType ObjectType { get; } = EntityObjectType.Activity;

        public string Id { get; set; }

        public Guid? DefinitionId { get; set; }

        public virtual ActivityDefinitionEntity Definition { get;set;}
    }
}
