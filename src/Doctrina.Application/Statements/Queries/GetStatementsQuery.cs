using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using MediatR;

namespace Doctrina.Application.Statements.Queries
{
    public class GetStatementsQuery : IRequest<ICollection<Statement>>
    {
    }
}
