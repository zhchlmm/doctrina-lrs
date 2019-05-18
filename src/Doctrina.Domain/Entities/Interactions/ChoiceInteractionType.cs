using System.Collections.Generic;

namespace Doctrina.Domain.Entities.Interactions
{
    public class ChoiceInteractionType : AbstractInteractionType
    {
        public ICollection<InteractionComponent> Choices { get; set; }
    }
}
