using FluentValidation;

namespace Doctrina.xAPI.Validation
{
    public class VerbValidator : AbstractValidator<Verb>
    {
        public VerbValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Display).SetValidator(new LanguageMapValidator()).When(verb=> verb.Display != null);
        }
    }
}
