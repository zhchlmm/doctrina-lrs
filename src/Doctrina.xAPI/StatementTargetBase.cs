using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Doctrina.xAPI
{
    [JsonConverter(typeof(StatementObjectConverter))]
    public abstract class StatementObjectBase : JsonModel, IObjectType
    {
        protected StatementObjectBase() { }
        protected StatementObjectBase(JObject jobj, ApiVersion version)
        {
        }

        protected virtual ObjectType OBJECT_TYPE { get; }

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
            string strObjectType = jobj["objectType"].Value<string>();

            if (strObjectType == ObjectType.StatementRef)
            {
                return new StatementRef(jobj, version);
            }

            if (strObjectType == ObjectType.SubStatement)
            {
                return new SubStatement(jobj, version);
            }

            if (strObjectType == ObjectType.Agent)
            {
                return new Agent(jobj, version);
            }

            if (strObjectType == ObjectType.Group)
            {
                return new Group(jobj, version);
            }

            return new Activity(jobj, version);
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
