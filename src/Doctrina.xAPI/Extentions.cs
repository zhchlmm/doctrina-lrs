using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Doctrina.xAPI.Json.Converters;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI
{
    [JsonConverter(typeof(ExtensionsConverter))]
    public class Extensions : JsonModel, ICollection<KeyValuePair<Uri, JToken>>
    {
        private IDictionary<Uri, JToken> _values = new Dictionary<Uri, JToken>();

        public Extensions() { }

        public Extensions(JObject jobj) :this(jobj, ApiVersion.GetLatest()) { }

        public Extensions(JObject jobj, ApiVersion version)
        {
            foreach(var token in jobj)
            {
                Add(new Uri(token.Key), token.Value);
            }
        }

        public int Count => _values.Count;

        public bool IsReadOnly => _values.IsReadOnly;

        public void Add(Uri key, JToken value)
        {
            _values.Add(new KeyValuePair<Uri, JToken>(key, value));
        }

        public void Add(KeyValuePair<Uri, JToken> item)
        {
            _values.Add(item);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(KeyValuePair<Uri, JToken> item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(KeyValuePair<Uri, JToken>[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<Uri, JToken> item)
        {
            return _values.Remove(item);
        }

        public IEnumerator<KeyValuePair<Uri, JToken>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject();
            foreach(var val in _values)
            {
                obj[val.Key] = val.Value;
            }
            return obj;
        }

        public static implicit operator Extensions(JObject jobj)
        {
            return new Extensions(jobj);
        }
    }
}
