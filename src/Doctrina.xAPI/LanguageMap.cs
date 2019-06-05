﻿using Doctrina.xAPI.Exceptions;
using Doctrina.xAPI.Helpers;
using Doctrina.xAPI.Json.Exceptions;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

        public LanguageMap(JToken languageMap, ApiVersion version)
        {
            GuardType(languageMap, JTokenType.Object);

            foreach (var item in (JObject)languageMap)
            {
                GuardType(item.Value, JTokenType.String);

                if(!CultureHelper.IsValidCultureName(item.Key))
                {
                    throw new CultureNameException(languageMap, item.Key);
                }

                //if (ContainsKey(item.Key))
                //{
                //    throw new LanguageMapKeyException();
                //}

                Add(item.Key, item.Value.Value<string>());
            }
        }

        private bool AllowCultureName(KeyValuePair<string, JToken> item)
        {
            var token = item.Value;
            if (token != null && token.Type == JTokenType.String)
            {
                if(CultureHelper.IsValidCultureName(item.Key))
                {
                    return true;
                }
            }

            ParsingErrors.Add(token.Path, "Invalid language code.");
            return false;
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
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