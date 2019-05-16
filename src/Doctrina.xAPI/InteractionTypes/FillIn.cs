using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class FillIn : InteractionTypeBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.FillIn;

        public FillIn()
        {
        }

        public FillIn(string jsonString) : base(jsonString)
        {
        }

        public FillIn(JObject jobj) : base(jobj)
        {
        }
        public FillIn(JObject jobj, ApiVersion version) : base(jobj, version)
        {
        }

    }
}
