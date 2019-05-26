using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class StatementValidator : AbstractValidator<Statement>
    {
        public StatementValidator()
        {
            Include(new StatementBaseValidator());

            RuleFor(x => x.Actor).NotNull();
            RuleFor(x => x.Verb).NotNull();
            RuleFor(x => x.Object).NotNull();
        }
    }
}
