using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
