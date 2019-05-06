using Doctrina.Domain.Entities.OwnedTypes;
using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class ActivityDefinitionEntity
    {
        public ICollection<LanguageMapEntity> Name { get; set; }

        public ICollection<LanguageMapEntity> Description { get; set; }

        public string Type { get; set; }

        public string MoreInfo { get; set; }

        public ICollection<ExtensionEntity> Extensions { get; set; }
    }
}
