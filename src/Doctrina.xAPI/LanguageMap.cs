using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonDictionary()]
    [JsonConverter(typeof(LanguageMapJsonConverter))]
    public class LanguageMap : JsonModel, IDictionary<string, string>
    {
        public IDictionary<string, string> _values = new Dictionary<string, string>();

        public LanguageMap() { }
        public LanguageMap(IEnumerable<KeyValuePair<string, string>> values)
        {
            
        }

        public LanguageMap(string jsonString) : this(JObject.Parse(jsonString)) { }

        public LanguageMap(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }

        public LanguageMap(JObject jobj, ApiVersion version)
        {
            foreach (var item in jobj)
            {
                Add(item.Key, item.Value.Value<string>());
            }
        }

        public string this[string key] { get => _values[key]; set => _values[key] = value; }

        public int Count => _values.Count;

        public bool IsReadOnly => throw new System.NotImplementedException();

        public ICollection<string> Keys => _values.Keys;

        public ICollection<string> Values => _values.Values;

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject();
            foreach (var pair in this)
            {
                obj[pair.Key] = pair.Value;
            }
            return obj;
        }

        //public static LanguageMap Parse(string jsonString)
        //{
        //    return Parse(jsonString, ApiVersion.GetLatest());
        //}

        //public static LanguageMap Parse(string jsonString, ApiVersion version)
        //{
        //    var obj = JObject.Parse(jsonString);
        //    var languageMap = new LanguageMap();
        //    foreach (var item in obj)
        //    {
        //        languageMap.Add(item.Key, item.Value.Value<string>());
        //    }
        //    return languageMap;
        //}

        //public static bool TryParse(string jsonString, out LanguageMap result)
        //{
        //    return TryParse(jsonString, ApiVersion.GetLatest(), out result);
        //}

        //public static bool TryParse(string json, ApiVersion version, out LanguageMap result)
        //{
        //    result = null;
        //    try
        //    {
        //        result = Parse(json, version);
        //        return true;
        //    }
        //    catch (System.Exception)
        //    {
        //        return false;
        //    }
        //}

        public void Add(string key, string value)
        {
            _values.Add(key, value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _values.Add(item);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _values.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _values.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _values.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return _values.Remove(item);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _values.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public static implicit operator LanguageMap(JObject jobj)
        {
            return new LanguageMap(jobj);
        }
    }
}