using Doctrina.xAPI;
using MediatR;
using System;

namespace Doctrina.Application.Statements.Queries
{
    public class GetStatementQuery : IRequest<Statement>
    {
        public bool IncludeAttachments { get; set; }

        public Guid StatementId { get; set; }    
    }
}
