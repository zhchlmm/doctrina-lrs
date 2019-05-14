using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace Doctrina.xAPI
{
    public class ObjectType
    {
        private static readonly ICollection<ObjectType> _types = new HashSet<ObjectType>();

        public static ObjectType Agent = new ObjectType("Agent");
        public static ObjectType Group = new ObjectType("Group");
        public static ObjectType Activity = new ObjectType("Activity");
        public static ObjectType SubStatement = new ObjectType("SubStatement");
        public static ObjectType StatementRef = new ObjectType("StatementRef");

        private readonly string _type;
        private ObjectType(string type)
        {
            _type = type;
            _types.Add(this);
        }

        public static implicit operator ObjectType(string type)
        {
            if (_types.Any(x=>x._type == type))
            {
                return new ObjectType(type);
            }

            throw new KeyNotFoundException();
        }

        public override string ToString()
        {
            return _type;
        }

        public static implicit operator string(ObjectType type)
        {
            return type.ToString();
        }
    }
}
