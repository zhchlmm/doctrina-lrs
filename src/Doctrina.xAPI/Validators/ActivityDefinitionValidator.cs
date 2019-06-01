using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class ActivityDefinitionValidator : AbstractValidator<ActivityDefinition>
    {
        public ActivityDefinitionValidator()
        {
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Description).SetValidator(new LanguageMapValidator()).When(x => x.Description != null);
            RuleFor(x => x.Name).SetValidator(new LanguageMapValidator()).When(x => x.Name != null);
            RuleFor(x => x.Extensions).SetValidator(new ExtensionsValidator()).When(x => x.Extensions != null);

            RuleFor(x => x.Failures).Custom((x, context) =>
            {
                foreach (var failure in x)
                {
                    context.AddFailure(failure.Name, failure.Message);
                }
            });
        }
    }
}
