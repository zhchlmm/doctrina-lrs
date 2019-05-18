using Doctrina.Domain.Entities.OwnedTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Doctrina.Domain.Entities.Interactions
{
    public class InteractionComponent
    {
        public string Id { get; set; }

        public ICollection<LanguageMapEntity> Description { get; set; }
    }
}
