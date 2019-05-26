using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.xAPI
{
    public class ObjectType
    {
        private static readonly ICollection<ObjectType> _types = new HashSet<ObjectType>();

        public static readonly ObjectType Agent = new ObjectType("Agent", typeof(Agent));
        public static readonly ObjectType Group = new ObjectType("Group", typeof(Group));
        public static readonly ObjectType Activity = new ObjectType("Activity", typeof(Activity));
        public static readonly ObjectType SubStatement = new ObjectType("SubStatement", typeof(SubStatement));
        public static readonly ObjectType StatementRef = new ObjectType("StatementRef", typeof(StatementRef));
        private static readonly ObjectType Statement = new ObjectType("Statement", typeof(Statement));

        public readonly string Alias;
        public readonly Type Type;

        private ObjectType(string alias, Type type)
        {
            Alias = alias;
            Type = type;
            _types.Add(this);
        }
        public IStatementObject CreateInstance()
        {
            return (IStatementObject)Activator.CreateInstance(Type);
        }

        public IStatementObject CreateInstance(JObject jobj, ApiVersion version)
        {
            return (IStatementObject)Activator.CreateInstance(Type, jobj, version);
        }

        public static implicit operator ObjectType(string type)
        {
            var objectType = _types.FirstOrDefault(x => x.Alias == type);
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

        public override bool Equals(object obj)
        {
            return obj is ObjectType type &&
                   Alias == type.Alias;
        }

        public override int GetHashCode()
        {
            var hashCode = 1278524668;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Alias);
            return hashCode;
        }

        public static implicit operator string(ObjectType type)
        {
            return type.ToString();
        }

        public static bool operator ==(ObjectType left, ObjectType right)
        {
            return left?.ToString() == right?.ToString();
        }

        public static bool operator !=(ObjectType left, ObjectType right)
        {
            return left?.ToString() != right?.ToString();
        }
    }
}
