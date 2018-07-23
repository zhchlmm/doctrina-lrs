using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Models
{
    public interface IStatementTarget
    {
        [JsonProperty("objectType", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        ObjectType ObjectType { get; }
    }
}
