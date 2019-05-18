using System.Collections.Generic;

namespace Doctrina.Domain.Entities.Interactions
{
    public class PerformanceInteractionType : AbstractInteractionType
    {
        public ICollection<InteractionComponent> Steps { get; set; }
    }
}
