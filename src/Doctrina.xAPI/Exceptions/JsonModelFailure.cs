using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Exceptions
{
    public class JsonModelFailure
    {
        public JsonModelFailure() { }
        public JsonModelFailure(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public string Name { get; private set; }
        public string Message { get; private set; }
    }
}
