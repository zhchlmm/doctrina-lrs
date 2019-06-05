using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.Json.Exceptions
{
    /// <summary>
    /// Unexpected <see cref="ObjectType"/> at token location.
    /// </summary>
    public class UnexpectedObjectTypeException : JsonTokenModelException
    {
        public UnexpectedObjectTypeException(JToken token, ObjectType objectType) 
            : base(token, $"'{objectType}' is not valid here.")
        {
        }
    }
}
