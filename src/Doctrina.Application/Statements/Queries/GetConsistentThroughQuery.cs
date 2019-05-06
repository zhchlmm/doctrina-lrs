using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Statements.Queries
{
    public class GetConsistentThroughQuery : IRequest<DateTime>
    {
        public class Handler : IRequestHandler<GetConsistentThroughQuery, DateTime>
        {
            public Task<DateTime> Handle(GetConsistentThroughQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
