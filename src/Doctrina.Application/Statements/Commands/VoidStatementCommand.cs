using Doctrina.Domain.Entities;
using MediatR;

namespace Doctrina.Application.Statements.Commands
{
    public class VoidStatementCommand : IRequest
    {
        public IStatementBaseEntity Statement { get; set; }
    }
}
