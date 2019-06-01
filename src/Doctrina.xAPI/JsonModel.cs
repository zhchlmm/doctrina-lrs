using Doctrina.xAPI.Collections;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Doctrina.xAPI
{
    public abstract class JsonModel : JsonModel<JObject> { }

    public abstract class JsonModel<TToken> : IJsonModel
        where TToken : JToken
    {
        public JsonModelFailuresCollection Failures { get; } = new JsonModelFailuresCollection();

        public abstract TToken ToJToken(ApiVersion version, ResultFormat format);

        public virtual string ToJson(ApiVersion version, ResultFormat format = ResultFormat.Exact)
        {
            return ToJToken(version, format).ToString(Newtonsoft.Json.Formatting.None);
        }

        public virtual string ToJson(ResultFormat format = ResultFormat.Exact)
        {
            return ToJson(ApiVersion.GetLatest(), format);
        }

        public override bool Equals(object obj)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void DisallowAdditionalProps(JObject jobj, params string[] allowedPropertyNames)
        {
            var disallowedProps = jobj.Properties()
                .Where(x => x.Name != null && !allowedPropertyNames.Contains(x.Name))
                .Select(x => x.Name);

            if (disallowedProps.Count() > 0)
            {
                Failures.Add(jobj.Path, $"Contains additional JSON properties \"{string.Join(",", disallowedProps)}\", which is not allowed.");
            }
        }

        public bool AllowString(JToken token)
        {
            if (token.Type == JTokenType.String)
            {
                return true;
            }

            Failures.Add(token.Path, $"'{token.ToString(Newtonsoft.Json.Formatting.None)}' is not a valid string.");
            return false;
        }

        public bool AllowNumber(JToken token)
        {
            if (token != null && (token.Type == JTokenType.Integer || token.Type == JTokenType.Float))
            {
                return true;
            }

            Failures.Add(token.Path, "Is not a valid JSON number.");
            return false;
        }

        public bool AllowBoolean(JToken token)
        {
            if (token != null && token.Type == JTokenType.Boolean)
            {
                return true;
            }

            Failures.Add(token.Path, "Is not a valid JSON boolean.");
            return false;
        }

        public bool AllowObject(JToken token)
        {
            if(token != null && token.Type == JTokenType.Object)
            {
                return true;
            }

            Failures.Add(token.Path, "Is not a valid JSON object.");
            return false;
        }

        public bool AllowDateTimeOffset(JToken token)
        {
            if (token == null || token.Type != JTokenType.String)
            {
                return false;
            }

            string strDateTime = token.Value<string>();

            if (strDateTime.EndsWith("-00:00")
            || strDateTime.EndsWith("-0000")
            || strDateTime.EndsWith("-00"))
            {
                Failures.Add(token.Path, $"'{strDateTime}' does not allow an offset of -00:00, -0000, -00");
            }

            if (DateTimeOffset.TryParse(strDateTime, out DateTimeOffset result))
            {
                return true;
            }
            else
            {
                Failures.Add(token.Path, $"'{strDateTime}' is not a well formed DateTime string.");
                return false;
            }
        }
        /// <summary>
        /// Adds failure if token type is null.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool DisallowNull(JToken token)
        {
            if (token == null)
            {
                return false;
            }

            if (token.Type == JTokenType.Null)
            {
                Failures.Add(token.Path, "Null values are not allowed.");
                return false;
            }

            return true;
        }

        public JsonModelFailuresCollection GetFailures()
        {
            var type = GetType();
            var models = type.GetProperties(BindingFlags.Public)
                .Where(x => x.PropertyType.IsAssignableFrom(typeof(IJsonModel)))
                .Select(x => x.GetValue(this))
                .Where(x => x != null)
                .Cast<IJsonModel>();

            var failures = models.SelectMany(x => x.Failures).Concat(Failures).ToList();

            return new JsonModelFailuresCollection(failures);
        }

        /// <summary>
        /// Uses a combination of the errors during parsing and abstract validator 
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return Failures.Count() == 0;
        }
    }
}
