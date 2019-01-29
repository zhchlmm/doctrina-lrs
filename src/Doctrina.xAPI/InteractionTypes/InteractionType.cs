using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Doctrina.xAPI.InteractionTypes
{
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InteractionType
    {
        [EnumMember(Value = "choice")]
        Choice,

        [EnumMember(Value = "fill-in")]
        FillIn,

        [EnumMember(Value = "likert")]
        Likert,

        [EnumMember(Value = "long-fill-in")]
        LongFillIn,

        [EnumMember(Value = "matching")]
        Matching,

        [EnumMember(Value = "numeric")]
        Numeric,

        [EnumMember(Value = "performance")]
        Performance,

        [EnumMember(Value = "sequencing")]
        Sequencing,

        [EnumMember(Value = "true-false")]
        TrueFalse,

        [EnumMember(Value = "other")]
        Other
    }
}
