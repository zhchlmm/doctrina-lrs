using System;

namespace Doctrina.xAPI.Exceptions
{
    public class StatementsResultMultipartException : Exception
    {
        public StatementsResultMultipartException(string message)
            : base(message)
        {
        }

        public StatementsResultMultipartException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
