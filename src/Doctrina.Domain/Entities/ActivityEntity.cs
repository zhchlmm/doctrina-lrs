using System;

namespace Doctrina.Domain.Entities
{
    public class ActivityEntity : IStatementObjectEntity
    {
        /// <summary>
        /// MD5 of <see cref="Id"/>
        /// </summary>
        public string ActivityEntityId { get; set; }

        /// <summary>
        /// Actual Iri
        /// </summary>
        public string ActivityId { get; set; }

        public EntityObjectType ObjectType { get; } = EntityObjectType.Activity;

        public Guid? DefinitionId { get; set; }

        public virtual ActivityDefinitionEntity Definition { get;set;}
    }
}
