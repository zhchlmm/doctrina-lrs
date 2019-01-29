using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Exceptions
{
    public class RequirementException : Exception
    {
        public string[] Requirement { get; private set; }

        public RequirementException(string message) 
            : base(message)
        {
        }

        public RequirementException(string message, params string[] requirement)
            : base(message)
        {
            Requirement = requirement;
        }
    }
}
