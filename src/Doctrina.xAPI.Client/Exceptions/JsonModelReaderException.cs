using System;

namespace Doctrina.xAPI.Client
{
    public class JsonModelReaderException : Exception
    {
        public JsonModelReaderException(string key, string message) : base(message)
        {
            Key = key;
        }

        public string Key { get; }
    }
}
