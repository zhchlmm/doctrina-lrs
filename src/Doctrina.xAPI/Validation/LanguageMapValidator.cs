using Doctrina.xAPI.Helpers;
using FluentValidation;

namespace Doctrina.xAPI.Validation
{
    public class LanguageMapValidator : AbstractValidator<LanguageMap>
    {
        public LanguageMapValidator()
        {
            RuleForEach(x => x).Must(x => CultureHelper.IsValidCultureName(x.Key))
                .WithMessage("Invalid culture name.");
        }
    }
}
