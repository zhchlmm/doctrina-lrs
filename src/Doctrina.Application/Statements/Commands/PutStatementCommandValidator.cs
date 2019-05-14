using FluentValidation;

namespace Doctrina.Application.Statements.Commands
{
    public class PutStatementCommandValidator : AbstractValidator<PutStatementCommand>
    {
        //private readonly IMediator _mediator;

        public PutStatementCommandValidator()
        {
            //_mediator = mediator;

            RuleFor(x => x.StatementId).NotEmpty();
            //RuleFor(x => x.Statement).CustomAsync(ValidateStatements);
        }

        //private Task ValidateStatements(Statement arg1, CustomContext arg2, CancellationToken arg3)
        //{
        //    _mediator.Send(GetStatementQuery.Create(this.))
        //}
    }
}
