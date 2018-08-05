using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Doctrina.xAPI.Models
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
