using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema.Generation;

namespace Doctrina.xAPI
{
    public abstract class StatementTargetBase : JsonModel, IStatementTarget
    {
        protected abstract ObjectType OBJECT_TYPE { get; }

        [JsonProperty("objectType",
            Order = 1,
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        [EnumDataType(typeof(ObjectType))]
        public ObjectType ObjectType { get { return this.OBJECT_TYPE; } }

        public override bool Equals(object obj)
        {
            var @base = obj as StatementTargetBase;
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

        public static bool operator ==(StatementTargetBase base1, StatementTargetBase base2)
        {
            return EqualityComparer<StatementTargetBase>.Default.Equals(base1, base2);
        }

        public static bool operator !=(StatementTargetBase base1, StatementTargetBase base2)
        {
            return !(base1 == base2);
        }
    }
}
