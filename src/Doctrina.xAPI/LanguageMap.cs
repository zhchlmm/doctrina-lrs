using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonDictionary()]
    [JsonConverter(typeof(LanguageMapJsonConverter))]
    public class LanguageMap : Dictionary<string, string>
    {
        public string ToJson(bool pretty = false) => JsonConvert.SerializeObject(this);

        //public override bool Equals(object obj)
        //{
        //    return false;
        //}

        //public override int GetHashCode()
        //{
        //    return -1;
        //}
    }
}