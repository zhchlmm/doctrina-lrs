using FluentValidation;

namespace Doctrina.Application.Statements.Commands
{
    public class CreateStatementCommandValidator : AbstractValidator<CreateStatementCommand>
    {
        public CreateStatementCommandValidator()
        {
        }
    }
}
