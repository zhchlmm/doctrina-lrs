using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonConverter(typeof(StatementObjectConverter))]
    public abstract class StatementObjectBase : JsonModel
    {
        protected StatementObjectBase() { }
        protected StatementObjectBase(JObject jobj, ApiVersion version)
        {
        }

        protected abstract ObjectType OBJECT_TYPE { get; }

        public ObjectType ObjectType { get { return this.OBJECT_TYPE; } }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            return new JObject
            {
                ["objectType"] = (string)ObjectType
            };
        }

        internal static IObjectType Parse(JObject jobj, ApiVersion version)
        {
            if (jobj["objectType"] != null)
            {
                ObjectType strObjectType = jobj.Value<string>("objectType");

                return strObjectType.CreateInstance(jobj, version);
            }

            // Assume activity
            return ObjectType.Activity.CreateInstance(jobj, version);
        }

        public override bool Equals(object obj)
        {
            var @base = obj as StatementObjectBase;
            return @base != null &&
                   base.Equals(obj) &&
                   OBJECT_TYPE == @base.OBJECT_TYPE;
        }

        public override int GetHashCode()
        {
            var hashCode = 1521006493;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + OBJECT_TYPE.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(StatementObjectBase base1, StatementObjectBase base2)
        {
            return EqualityComparer<StatementObjectBase>.Default.Equals(base1, base2);
        }

        public static bool operator !=(StatementObjectBase base1, StatementObjectBase base2)
        {
            return !(base1 == base2);
        }
    }
}
