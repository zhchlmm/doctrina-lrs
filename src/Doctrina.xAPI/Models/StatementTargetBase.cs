using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema.Generation;

namespace Doctrina.xAPI.Models
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
    }
}
