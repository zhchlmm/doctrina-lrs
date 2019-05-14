using Doctrina.Domain.Entities;
using MediatR;

namespace Doctrina.Application.Statements.Commands
{
    public class VoidStatementCommand : IRequest
    {
        public StatementBaseEntity Statement { get; set; }
    }
}
