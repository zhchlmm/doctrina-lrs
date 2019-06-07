using System;

namespace Doctrina.xAPI
{
    public class MultipartSectionException : Exception
    {
        public MultipartSectionException(string message)
            : base(message)
        {
        }
    }
}
