namespace Doctrina.Domain.Entities
{
    public class ActivityEntity : IStatementObjectEntity
    {
        public EntityObjectType ObjectType => EntityObjectType.Activity;

        /// <summary>
        /// Hash of <see cref="Id"/>
        /// </summary>
        public string ActivityHash { get; set; }

        /// <summary>
        /// Actual Iri
        /// </summary>
        public string ActivityId { get; set; }

        public ActivityDefinitionEntity Definition { get; set; }
    }
}
