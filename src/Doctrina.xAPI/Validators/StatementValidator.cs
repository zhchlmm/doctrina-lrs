using FluentValidation;

namespace Doctrina.xAPI.Validators
{
    public class StatementValidator : AbstractValidator<Statement>
    {
        public StatementValidator()
        {
            Include(new StatementBaseValidator());
        }
    }
}
