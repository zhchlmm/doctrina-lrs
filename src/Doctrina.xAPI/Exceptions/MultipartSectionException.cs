using System;

namespace Doctrina.xAPI.Consumer.Exceptions
{
    public class MultipartSectionException : Exception
    {
        public MultipartSectionException(string message)
            : base(message)
        {
        }
    }
}
