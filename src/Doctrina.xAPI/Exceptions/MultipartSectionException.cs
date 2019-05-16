using System;

namespace Doctrina.xAPI.Exceptions
{
    public class MultipartSectionException : Exception
    {
        public MultipartSectionException(string message)
            : base(message)
        {
        }
    }
}
