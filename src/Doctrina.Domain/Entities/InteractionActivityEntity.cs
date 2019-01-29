using Doctrina.Domain.Entities.DataTypes;
using System.Collections;
using System.Collections.Generic;

namespace Doctrina.Domain.Entities
{
    public class InteractionActivity : ActivityDefinitionEntity
    {
        public string InteractionType { get; set; }

        public string[] CorrectResponsesPattern { get; set; }
    }

    public class ChoiceInteractionType : InteractionActivity
    {
        public InteractionComponentCollection Choices { get; set; }
    }

    public class LikertInteractionType : InteractionActivity
    {
        public InteractionComponentCollection Scale { get; set; }
    }

    public class MatchingInteractionType : InteractionActivity
    {
        public InteractionComponentCollection Source { get; set; }
        public InteractionComponentCollection Target { get; set; }
    }

    public class PerformanceInteractionType : InteractionActivity
    {
        public InteractionComponentCollection Steps { get; set; }
    }
}
