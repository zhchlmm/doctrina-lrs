using Newtonsoft.Json;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Sequencing : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Sequencing;

        [JsonProperty("choices")]
        public InteractionComponent[] Choices { get; set; }
    }
}
