using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class ActivityDefinitionValidator : AbstractValidator<ActivityDefinition>
    {
        public ActivityDefinitionValidator()
        {
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Description).SetValidator(new LanguageMapValidator());
            RuleFor(x => x.Name).SetValidator(new LanguageMapValidator());
            RuleFor(x => x.Extentions).SetValidator(new ExtensionsValidator());
            //RuleFor(x => x.MoreInfo).
        }
    }
}
