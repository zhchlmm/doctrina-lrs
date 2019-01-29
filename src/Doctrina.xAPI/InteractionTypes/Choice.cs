using Newtonsoft.Json;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Choice : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Choice;

        [JsonProperty("choices")]
        public InteractionComponent[] Choices { get; set; }
    }
}
