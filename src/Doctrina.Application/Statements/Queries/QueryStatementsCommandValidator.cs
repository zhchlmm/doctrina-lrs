using FluentValidation;

namespace Doctrina.Application.Statements.Queries
{
    public class QueryStatementsCommandValidator : AbstractValidator<GetStatementsQuery>
    {
        public QueryStatementsCommandValidator()
        {
        }
    }
}
