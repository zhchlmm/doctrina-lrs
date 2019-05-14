using Doctrina.Domain.Entities.OwnedTypes;
using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class VerbEntity
    {
        /// <summary>
        /// SHA-1 of <see cref="Id"/>
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// Corresponds to a Verb definition. (IRI)
        /// </summary>
        public string Id { get; set; }

        public ICollection<LanguageMapEntity> Display { get; set; }
    }
}
