using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class InteractionType
    {
        public static readonly ICollection<InteractionType> Types = new HashSet<InteractionType>();

        public static readonly InteractionType Choice = new InteractionType("choice", typeof(Choice));
        public static readonly InteractionType FillIn = new InteractionType("fill-in", typeof(FillIn));
        public static readonly InteractionType Likert = new InteractionType("likert", typeof(Likert));
        public static readonly InteractionType LongFillIn = new InteractionType("long-fill-in", typeof(LongFillIn));
        public static readonly InteractionType Matching = new InteractionType("matching", typeof(Matching));
        public static readonly InteractionType Numeric = new InteractionType("numeric", typeof(Numeric));
        public static readonly InteractionType Performance = new InteractionType("performance", typeof(Performance));
        public static readonly InteractionType Sequencing = new InteractionType("sequencing", typeof(Sequencing));
        public static readonly InteractionType TrueFalse = new InteractionType("true-false", typeof(TrueFalse));
        public static readonly InteractionType Other = new InteractionType("other", typeof(Other));

        public readonly string Alias;
        public readonly Type Type;

        private InteractionType(string strType, Type type)
        {
            Alias = strType;
            Type = type;
            Types.Add(this);
        }

        public ActivityDefinition CreateInstance()
        {
            return (ActivityDefinition)Activator.CreateInstance(Type);
        }

        public ActivityDefinition CreateInstance(JObject jobj, ApiVersion version)
        {
            return (ActivityDefinition)Activator.CreateInstance(Type, jobj, version);
        }
        public override bool Equals(object obj)
        {
            return obj is InteractionType type &&
                   Alias == type.Alias;
        }

        public override int GetHashCode()
        {
            return -331038658 + EqualityComparer<string>.Default.GetHashCode(Alias);
        }

        public static bool operator ==(InteractionType type1, InteractionType type2)
        {
            return type1.Alias == type2.Alias;
        }

        public static bool operator !=(InteractionType type1, InteractionType type2)
        {
            return type1.Alias != type2.Alias;
        }

        public static implicit operator InteractionType(string alias)
        {
            var interactionType = Types.FirstOrDefault(x => x.Alias == alias);
            if (interactionType != null)
            {
                return interactionType;
            }

            throw new KeyNotFoundException();
        }

        public static implicit operator string(InteractionType type)
        {
            return type.Alias;
        }
    }
}
