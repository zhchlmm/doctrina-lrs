using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Doctrina.xAPI.Json
{
    public class InvalidTokenTypeException : JsonTokenModelException
    {
        public JTokenType[] Types;

        public InvalidTokenTypeException(JToken token, JTokenType type)
            : base (token, $"Expected JSON type '{Enum.GetName(typeof(JTokenType), type)}', but received '{Enum.GetName(typeof(JTokenType), token.Type)}'.")
        {
            Types = new[] { type };
        }

        public InvalidTokenTypeException(JToken token, JTokenType[] types)
            : base(token, $"Expected JSON types '{string.Join(", ", types.Select(x=> Enum.GetName(typeof(JTokenType), x)).ToArray())}', but received '{Enum.GetName(typeof(JTokenType), token.Type)}'.")
        {
            Types = types;
        }
    }
}
