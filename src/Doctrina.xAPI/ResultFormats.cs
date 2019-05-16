using System.Runtime.Serialization;

namespace Doctrina.xAPI
{
    public enum ResultFormat
    {
        [EnumMember(Value = "ids")]
        Ids = 0,

        [EnumMember(Value = "exact")]
        Exact = 1,

        [EnumMember(Value = "cannonical")]
        Cannonical = 2
    }
}
