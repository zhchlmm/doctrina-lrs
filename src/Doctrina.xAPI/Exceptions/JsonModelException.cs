using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Exceptions
{
    public class JsonModelException : Exception
    {
        public JsonModelException(string message) : base(message)
        {
        }
    }
}
