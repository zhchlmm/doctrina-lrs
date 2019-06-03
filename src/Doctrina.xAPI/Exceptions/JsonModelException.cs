using System;

namespace Doctrina.xAPI.Exceptions
{
    public class JsonModelException : Exception
    {
        public JsonModelException(string message) : base(message)
        {
        }
    }
}
