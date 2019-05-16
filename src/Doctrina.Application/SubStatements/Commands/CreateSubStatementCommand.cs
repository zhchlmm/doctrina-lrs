using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using MediatR;

namespace Doctrina.Application.SubStatements.Commands
{
    public class CreateSubStatementCommand : IRequest<SubStatementEntity>
    {
        public SubStatement SubStatement { get; set; }
    }
}
