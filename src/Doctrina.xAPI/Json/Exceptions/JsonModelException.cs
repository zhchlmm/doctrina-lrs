using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.xAPI.Json.Exceptions
{
    public class JsonModelException : Exception
    {
        public JsonModelException(string message)
            : base(message)
        {
        }
    }

    public class JsonTokenModelException : JsonModelException
    {
        public JsonTokenModelException(JToken token, string message) 
            : base($"{message.EnsureEndsWith(".")} Path: '{token.Path}'")
        {
            Token = token;
        }

        public JToken Token { get; }
    }
}
