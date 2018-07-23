using Doctrina.xAPI.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Doctrina.xAPI.Models
{
    [JsonConverter(typeof(LanguageMapConverter))]
    public class LanguageMap : Dictionary<string, string>, ILanguageMap
    {
        //private Dictionary<string, string> _maps;

        //public string this[string languageCode]
        //{
        //    get
        //    {
        //        return _maps[languageCode];
        //    }
        //    set
        //    {
        //        _maps[languageCode] = value;
        //    }
        //}

        public string ToJson(bool pretty = false)
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}