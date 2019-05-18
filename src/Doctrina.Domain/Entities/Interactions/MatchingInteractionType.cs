using System.Collections.Generic;

namespace Doctrina.Domain.Entities.Interactions
{
    public class MatchingInteractionType : AbstractInteractionType
    {
        public ICollection<InteractionComponent> Source { get; set; }
        public ICollection<InteractionComponent> Target { get; set; }
    }
}
