using Doctrina.xAPI;
using MediatR;
using System;

namespace Doctrina.Application.Statements.Commands
{
    public class CreateStatementCommand : IRequest<Guid>
    {
        public Statement Statement { get; set; }

        public static CreateStatementCommand Create(Statement statement)
        {
            return new CreateStatementCommand()
            {
                Statement = statement
            };
        }
    }
}
