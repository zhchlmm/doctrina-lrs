using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Models
{
    [JsonObject]
    public class StatementRef : StatementTargetBase
    {
        protected override ObjectType OBJECT_TYPE => ObjectType.StatementRef;

        /// <summary>
        /// The UUID of a Statement.
        /// </summary>
        [JsonProperty("id",
            Required = Required.Always)]
        public Guid Id { get; set; }
    }
}
