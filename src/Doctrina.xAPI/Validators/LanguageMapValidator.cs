using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class LanguageMapValidator : AbstractValidator<LanguageMap>
    {
        public LanguageMapValidator()
        {
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
