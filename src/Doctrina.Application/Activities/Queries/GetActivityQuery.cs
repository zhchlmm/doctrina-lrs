using Doctrina.xAPI;
using MediatR;

namespace Doctrina.Application.Activities.Queries
{
    public class GetActivityQuery : IRequest<Activity>
    {
        public Iri ActivityId { get; set; }
    }
}
