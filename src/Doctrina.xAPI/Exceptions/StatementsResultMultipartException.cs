using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
