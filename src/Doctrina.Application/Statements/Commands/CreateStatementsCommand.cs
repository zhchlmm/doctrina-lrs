using AutoMapper;
using Doctrina.Persistence;
using Doctrina.xAPI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
