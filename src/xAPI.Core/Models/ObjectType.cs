using xAPI.Core.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Models
{
    [JsonConverter(typeof(ObjectTypeConverter))]
    public enum ObjectType
    {
        [EnumMember(Value = "Agent")]
        Agent,

        [EnumMember(Value = "Group")]
        Group,

        [EnumMember(Value = "Activity")]
        Activity,

        [EnumMember(Value = "SubStatement")]
        SubStatement,

        [EnumMember(Value = "StatementRef")]
        StatementRef
    }
}
