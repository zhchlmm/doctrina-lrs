using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Doctrina.xAPI
{
    public class LanguageMap : JsonModel, IDictionary<string, string>
    {
        public IDictionary<string, string> _values = new Dictionary<string, string>();

        public LanguageMap() { }
        public LanguageMap(IEnumerable<KeyValuePair<string, string>> values)
        {
            foreach(var item in values)
            {
                Add(item);
            }
        }

        public LanguageMap(JsonString jsonString) : this(jsonString.ToJToken()) { }

        public LanguageMap(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }

        public LanguageMap(JToken jtoken, ApiVersion version)
        {
            if (!AllowObject(jtoken))
            {
                return;
            }

            var jobj = jtoken as JObject;

            foreach (var item in jobj)
            {
                if (DisallowNull(item.Value) && AllowCultureName(item))
                {
                    if (ContainsKey(item.Key))
                    {
                        Failures.Add(item.Value.Path, "Duplicate language code key.");
                        continue;
                    }

                    Add(item.Key, item.Value.Value<string>());
                }
            }
        }

        private bool AllowCultureName(KeyValuePair<string, JToken> item)
        {
            var token = item.Value;
            if (token != null && token.Type == JTokenType.String)
            {
                if(item.Key == "und" || IsValidCultureName(item.Key))
                {
                    return true;
                }
            }

            Failures.Add(token.Path, "Invalid language code.");
            return false;
        }

        private bool IsValidCultureName(string cultureName)
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo culture in cultures)
            {
                if (culture.Name == cultureName)
                {
                    return true;
                }
            }

            return false;
        }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject();
            foreach (var pair in this)
            {
                obj[pair.Key] = pair.Value;
            }
            return obj;
        }

        public static implicit operator LanguageMap(JObject jobj)
        {
            return new LanguageMap(jobj);
        }

        #region Implementation
        public string this[string key] { get => _values[key]; set => _values[key] = value; }

        public int Count => _values.Count;

        public bool IsReadOnly => throw new System.NotImplementedException();

        public ICollection<string> Keys => _values.Keys;

        public ICollection<string> Values => _values.Values;

        

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
        #endregion
    }
}