using Doctrina.xAPI.Collections;
using Doctrina.xAPI.Exceptions;
using Doctrina.xAPI.Json.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Doctrina.xAPI
{
    public abstract class JsonModel : JsonModel<JToken>
    {
        public JsonModel() : base() { }

        public JsonModel(JToken token, ApiVersion version) : base(token, version)
        {
        }
    }

    public abstract class JsonModel<TToken> : IJsonModel
        where TToken : JToken
    {
        public JsonModel() { }
        public JsonModel(TToken token, ApiVersion version) { }

        public JsonModelErrorsCollection ParsingErrors { get; } = new JsonModelErrorsCollection();

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

        public void GuardAdditionalProperties(JObject jobj, params string[] allowedPropertyNames)
        {
            var disallowedProps = jobj.Properties()
                .Where(x => x.Name != null && !allowedPropertyNames.Contains(x.Name))
                .Select(x => x.Name);

            if (disallowedProps.Count() > 0)
            {
                ParsingErrors.Add(jobj.Path, $"Contains additional JSON properties \"{string.Join(",", disallowedProps)}\", which is not allowed.");
            }
        }

        public DateTimeOffset ParseDateTimeOffset(JToken token)
        {
            GuardType(token, JTokenType.String);

            string strDateTime = token.Value<string>();

            if (strDateTime.EndsWith("-00:00")
            || strDateTime.EndsWith("-0000")
            || strDateTime.EndsWith("-00"))
            {
                throw new InvalidDateTimeOffsetException(token, strDateTime);
            }

            if (DateTimeOffset.TryParse(strDateTime, out DateTimeOffset result))
            {
                return result;
            }
            else
            {
                throw new InvalidDateTimeOffsetFormatException(token, strDateTime);
            }
        }

        public Guid ParseGuid(JToken token)
        {
            GuardType(token, JTokenType.String);

            string strGuid = token.Value<string>();

            if(Guid.TryParse(strGuid, out Guid result))
            {
                return result;
            }
            else
            {
                throw new GuidFormatException(token, strGuid);
            }
        }

        public ObjectType ParseObjectType(JToken token, params ObjectType[] types)
        {
            GuardType(token, JTokenType.String);
            try
            {
                ObjectType objectType = token.Value<string>();
                foreach (var type in types)
                {
                    if (objectType == type)
                    {
                        return objectType;
                    }
                }

                throw new UnexpectedObjectTypeException(token, objectType);
            }
            catch (InvalidObjectTypeException ex)
            {
                throw new JsonTokenModelException(token, ex.Message);
            }
        }

        /// <summary>
        /// Throws an exception when type does not match any of the provided types.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="types"></param>
        public void GuardType(JToken token, params JTokenType[] types)
        {
            if(!types.Contains(token.Type))
            {
                if(types.Length == 1)
                {
                    throw new InvalidTokenTypeException(token, types[0]);
                }
                else
                {
                    throw new InvalidTokenTypeException(token, types);
                }
            }
        }

        /// <summary>
        /// Gets errors from all descendants <see cref="JsonModel"/>'s and self.
        /// </summary>
        /// <returns></returns>
        public JsonModelErrorsCollection GetErrorsOfDescendantsAndSelf()
        {
            var type = GetType();
            var models = type.GetProperties(BindingFlags.Public)
                .Where(x => x.PropertyType.IsAssignableFrom(typeof(IJsonModel)))
                .Select(x => x.GetValue(this))
                .Where(x => x != null)
                .Cast<IJsonModel>();

            var failures = models.SelectMany(x => x.ParsingErrors).Concat(ParsingErrors).ToList();

            return new JsonModelErrorsCollection(failures);
        }

        /// <summary>
        /// Uses a combination of the errors during parsing and abstract validator
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return ParsingErrors.Count() == 0;
        }
    }
}
