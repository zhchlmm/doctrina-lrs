using Doctrina.Domain.Entities.OwnedTypes;
using System;
using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class ActivityDefinitionEntity
    {
        public Guid ActivityDefinitionId { get; set; }
        public ICollection<LanguageMapEntity> Names { get; set; }

        public ICollection<LanguageMapEntity> Descriptions { get; set; }

        public string Type { get; set; }

        public string MoreInfo { get; set; }

        public ICollection<ExtensionEntity> Extensions { get; set; }

        public string ActivityHash { get; set; }
        public virtual ActivityEntity Activity { get; set; }
    }
}
