using Doctrina.xAPI;
using MediatR;
using System;
using System.Collections.Generic;

namespace Doctrina.Application.Statements.Commands
{
    public class CreateStatementsCommand : IRequest<ICollection<Guid>>
    {
        public CreateStatementsCommand()
        {
            Statements = new HashSet<Statement>();
        }

        public ICollection<Statement> Statements { get; internal set; }

        public static CreateStatementsCommand Create(params Statement[] statements)
        {
            return new CreateStatementsCommand()
            {
                Statements = statements
            };
        }
    }
}
