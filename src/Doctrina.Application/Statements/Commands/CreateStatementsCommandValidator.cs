using Doctrina.xAPI.Validators;
using FluentValidation;

namespace Doctrina.Application.Statements.Commands
{
    public class CreateStatementsCommandValidator : AbstractValidator<CreateStatementsCommand>
    {
        public CreateStatementsCommandValidator()
        {
            //RuleForEach(x => x.Statements).SetValidator(new StatementValidator());
        }
    }
}
