using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.Json.Exceptions
{
    public class InvalidDateTimeOffsetFormatException : JsonTokenModelException
    {
        public InvalidDateTimeOffsetFormatException(JToken token, string date) 
            : base(token, $"'{date}' is not a well formed DateTimeOffset.")
        {
        }
    }
}
