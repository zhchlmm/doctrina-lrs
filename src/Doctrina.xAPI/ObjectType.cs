using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.xAPI
{
    public class ObjectType
    {
        public static readonly ICollection<ObjectType> Types = new HashSet<ObjectType>();

        public static readonly ObjectType Agent = new ObjectType("Agent", typeof(Agent));
        public static readonly ObjectType Group = new ObjectType("Group", typeof(Group));
        public static readonly ObjectType Activity = new ObjectType("Activity", typeof(Activity));
        public static readonly ObjectType SubStatement = new ObjectType("SubStatement", typeof(SubStatement));
        public static readonly ObjectType StatementRef = new ObjectType("StatementRef", typeof(StatementRef));

        public readonly string Alias;
        public readonly Type Type;

        private ObjectType(string alias, Type type)
        {
            Alias = alias;
            Type = type;
            Types.Add(this);
        }
        public IObjectType CreateInstance()
        {
            return (IObjectType)Activator.CreateInstance(Type);
        }

        public IObjectType CreateInstance(JObject jobj, ApiVersion version)
        {
            return (IObjectType)Activator.CreateInstance(Type, jobj, version);
        }

        public static implicit operator ObjectType(string type)
        {
            var objectType = Types.FirstOrDefault(x => x.Alias == type);
            if (objectType != null)
            {
                return objectType;
            }

            throw new KeyNotFoundException();
        }

        public override string ToString()
        {
            return Alias;
        }

        public static implicit operator string(ObjectType type)
        {
            return type.ToString();
        }
    }
}
