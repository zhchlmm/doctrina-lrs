using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Doctrina.xAPI.Models
{
    [JsonDictionary()]
    [JsonConverter(typeof(LanguageMapJsonConverter))]
    public class LanguageMap : Dictionary<string, string>, ILanguageMap
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