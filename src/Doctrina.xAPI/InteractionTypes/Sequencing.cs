using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Sequencing : InteractionTypeBase
    {
        public Sequencing() { }

        public Sequencing(JToken jtoken, ApiVersion version) : base(jtoken, version)
        {
        }

        protected override InteractionType INTERACTION_TYPE => InteractionType.Sequencing;

        public InteractionComponent[] Choices { get; set; }
    }
}
