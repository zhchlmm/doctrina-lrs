using Doctrina.xAPI.Helpers;
using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class LanguageMapValidator : AbstractValidator<LanguageMap>
    {
        public LanguageMapValidator()
        {
            RuleFor(x => x.ParsingErrors).Custom((x, context) =>
            {
                foreach (var failure in x)
                {
                    context.AddFailure(failure.Name, failure.ErrorMessage);
                }
            });

            RuleForEach(x => x).Must(x => CultureHelper.IsValidCultureName(x.Key))
                .WithMessage("Invalid culture name.");
        }
    }
}
