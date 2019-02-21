using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class VerbValidator : AbstractValidator<Verb>
    {
        public VerbValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
