using Doctrina.xAPI;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Activities.Queries
{
    public class GetActivityQuery : IRequest<Activity>
    {
        public Iri ActivityId { get; set; }

        public class Handler : IRequestHandler<GetActivityQuery, Activity>
        {
            public Task<Activity> HandleAsync(GetActivityQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
