using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.Json
{
    public class GuidFormatException : JsonTokenModelException
    {
        public GuidFormatException(JToken token, string guid) 
            : base(token, $"'{guid}' is not a valid UUID.")
        {
        }
    }
}
