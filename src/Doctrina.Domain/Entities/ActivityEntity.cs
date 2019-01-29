using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Domain.Entities
{
    public class ActivityEntity : IStatementObjectEntity
    {
        public Guid Key { get; set; }

        public EntityObjectType ObjectType { get; } = EntityObjectType.Activity;

        public string Id { get; set; }

        public Guid? DefinitionId { get; set; }

        public virtual ActivityDefinitionEntity Definition { get;set;}
    }
}
