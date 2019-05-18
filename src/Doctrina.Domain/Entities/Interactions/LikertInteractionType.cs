using System.Collections.Generic;

namespace Doctrina.Domain.Entities.Interactions
{
    public class LikertInteractionType : AbstractInteractionType
    {
        public ICollection<InteractionComponent> Scale { get; set; }
    }
}
