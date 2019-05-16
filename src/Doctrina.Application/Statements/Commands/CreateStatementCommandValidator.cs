using Doctrina.xAPI.Validators;
using FluentValidation;

namespace Doctrina.Application.Statements.Commands
{
    public class CreateStatementCommandValidator : AbstractValidator<CreateStatementCommand>
    {
        public CreateStatementCommandValidator()
        {
            RuleFor(x => x.Statement).SetValidator(new StatementValidator());
        }
    }
}
