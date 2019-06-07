using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.Json
{
    public class InvalidDateTimeOffsetException : JsonTokenModelException
    {
        public InvalidDateTimeOffsetException(JToken token, string date)
            : base(token, $"'{date}' does not allow an offset of -00:00, -0000, -00")
        {
        }
    }
}
