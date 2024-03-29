﻿using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class Numeric : InteractionTypeBase
    {
        public Numeric() { }

        public Numeric(JToken jobj, ApiVersion version) : base(jobj, version)
        {
        }

        protected override InteractionType INTERACTION_TYPE => InteractionType.Numeric;
    }
}
