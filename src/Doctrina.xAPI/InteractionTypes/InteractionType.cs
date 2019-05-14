using System.Collections.Generic;
using System.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class InteractionType
    {
        public static readonly ICollection<InteractionType> Types = new HashSet<InteractionType>();

        public static readonly InteractionType Choice = new InteractionType("choice");
        public static readonly InteractionType FillIn = new InteractionType("fill-in");
        public static readonly InteractionType Likert = new InteractionType("likert");
        public static readonly InteractionType LongFillIn = new InteractionType("long-fill-in");
        public static readonly InteractionType Matching = new InteractionType("matching");
        public static readonly InteractionType Numeric = new InteractionType("numeric");
        public static readonly InteractionType Performance = new InteractionType("performance");
        public static readonly InteractionType Sequencing = new InteractionType("sequencing");
        public static readonly InteractionType TrueFalse = new InteractionType("true-false");
        public static readonly InteractionType Other = new InteractionType("other");

        private readonly string _type;

        private InteractionType(string type)
        {
            _type = type;
            Types.Add(this);
        }

        public override bool Equals(object obj)
        {
            return obj is InteractionType type &&
                   _type == type._type;
        }

        public override int GetHashCode()
        {
            return -331038658 + EqualityComparer<string>.Default.GetHashCode(_type);
        }

        public static bool operator ==(InteractionType type1, InteractionType type2)
        {
            return type1._type == type2._type;
        }

        public static bool operator !=(InteractionType type1, InteractionType type2)
        {
            return type1._type != type2._type;
        }

        public static implicit operator InteractionType(string type)
        {
            if(Types.Any(x=>x._type == type))
            {
                return new InteractionType(type);
            }

            throw new KeyNotFoundException();
        }

        public static implicit operator string(InteractionType type)
        {
            return type._type;
        }
    }
}
