using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Exceptions
{
    public class JsonModelError
    {
        public JsonModelError() { }
        public JsonModelError(string name, string message)
        {
            Name = name;
            ErrorMessage = message;
        }

        public string Name { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}
