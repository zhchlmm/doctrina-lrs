using Doctrina.Domain.Entities.DataTypes;

namespace Doctrina.Domain.Entities
{
    public class VerbEntity
    {
        /// <summary>
        /// SHA-1 of <see cref="Id"/>
        /// </summary>
        public string VerbId { get; set; }

        /// <summary>
        /// Corresponds to a Verb definition. (IRI)
        /// </summary>
        public string Id { get; set; }

        public LanguageMapCollection Display { get; set; }
    }
}
