using Doctrina.xAPI;
using MediatR;
using System.Collections.Generic;

namespace Doctrina.Application.Statements.Queries
{
    public class GetStatementsQuery : IRequest<ICollection<Statement>>
    {
    }
}
