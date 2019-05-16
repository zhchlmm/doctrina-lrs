using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Statements.Queries
{
    public class GetConsistentThroughQuery : IRequest<DateTimeOffset>
    {
    }
}
