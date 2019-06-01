using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class VerbValidator : AbstractValidator<Verb>
    {
        public VerbValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Display).SetValidator(new LanguageMapValidator()).When(verb=> verb.Display != null);

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
