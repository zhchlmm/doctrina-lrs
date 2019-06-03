using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class FillIn : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.FillIn;

        public FillIn()
        {
        }

        public FillIn(JToken jobj, ApiVersion version) : base(jobj, version)
        {
        }

    }
}
