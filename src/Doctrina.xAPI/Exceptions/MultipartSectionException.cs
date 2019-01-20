using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
