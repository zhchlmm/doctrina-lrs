namespace Doctrina.Domain.Entities.Interactions
{
    public abstract class AbstractInteractionType : ActivityDefinitionEntity
    {
        public string InteractionType { get; set; }

        public string[] CorrectResponsesPattern { get; set; }
    }
}
