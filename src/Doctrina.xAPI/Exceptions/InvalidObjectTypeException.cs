using System;

namespace Doctrina.xAPI.Exceptions
{
    public class InvalidObjectTypeException : Exception
    {
        public InvalidObjectTypeException(string type) 
            : base($"'{type}' is not a valid ObjectType.")
        {
        }
    }
}
