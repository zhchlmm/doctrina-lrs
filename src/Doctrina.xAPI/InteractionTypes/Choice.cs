using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Choice : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Choice;
        public Choice() {}
        public Choice(JToken jobj, ApiVersion version) : base(jobj, version)
        {
            if (jobj["choices"] != null)
            {
                Choices = new InteractionComponentCollection(jobj["choices"], version);
            }
        }

        public InteractionComponentCollection Choices { get; set; }
    }
}
