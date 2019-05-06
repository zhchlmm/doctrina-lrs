using System.Threading;
using System.Threading.Tasks;
using Doctrina.Domain.Entities;
using MediatR;

namespace Doctrina.Application.Statements.Queries
{
    public class QueryStatementsCommand : IRequest<StatementEntity[]>
    {
        public class Handler : IRequestHandler<QueryStatementsCommand, StatementEntity[]>
        {
            public Task<StatementEntity[]> Handle(QueryStatementsCommand request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
