using System.Collections.Generic;

namespace Doctrina.Domain.Entities.Interactions
{
    public class SequencingInteractionType : AbstractInteractionType
    {
        public ICollection<InteractionComponent> Choices { get; set; }
    }
}
