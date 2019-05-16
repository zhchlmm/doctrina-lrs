using MediatR;
using System;

namespace Doctrina.Application.Statements.Queries
{
    public class GetConsistentThroughQuery : IRequest<DateTimeOffset?>
    {
    }
}
