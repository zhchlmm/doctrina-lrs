using System;

namespace Doctrina.xAPI.Json
{
    public class JsonModelException : Exception
    {
        public JsonModelException(string message)
            : base(message)
        {
        }
    }
}
