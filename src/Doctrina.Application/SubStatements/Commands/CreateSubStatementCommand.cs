using AutoMapper;
using Doctrina.Application.Interfaces.Mapping;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Application.SubStatements.Commands
{
    public class CreateSubStatementCommand : IRequest<SubStatementEntity>
    {
        public SubStatement SubStatement { get; set; }
    }
}
