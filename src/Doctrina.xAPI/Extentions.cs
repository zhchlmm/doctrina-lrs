using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Doctrina.xAPI.Json.Converters;

namespace Doctrina.xAPI
{
    [JsonConverter(typeof(ExtensionsConverter))]
    public class Extensions : Dictionary<Uri, object>
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string ToJson()
        {
            return this.ToString();
        }
    }
}
