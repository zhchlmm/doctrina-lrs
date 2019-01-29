using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.Json.Serialization
{
    public class JsonValidator
    {
        public static void IsString(JToken token)
        {
            if(token.Type != JTokenType.String)
            {
                throw new JsonSerializationException("Is not a valid string.");
            }
        }

        public static void IsNumber(JToken token)
        {
            if (token.Type != JTokenType.Integer && token.Type != JTokenType.Float)
            {
                throw new JsonSerializationException("Is not a valid number.");
            }
        }
    }
}
