using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Other : InteractionTypeBase
    {
        public Other()
        {
        }

        public Other(JToken jobj, ApiVersion version) : base(jobj, version)
        {
        }

        protected override InteractionType INTERACTION_TYPE => InteractionType.Other;
    }
}
