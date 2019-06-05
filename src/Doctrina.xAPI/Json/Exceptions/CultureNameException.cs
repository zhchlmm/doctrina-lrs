using Doctrina.xAPI.Json.Exceptions;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Exceptions
{
    public class CultureNameException : JsonTokenModelException
    {
        public CultureNameException(JToken token, string cultureName)
            : base(token, $"'{cultureName}' is not a valid RFC5646 Language Tag.")
        {
        }
    }
}
