using Doctrina.xAPI;
using MediatR;
using System;

namespace Doctrina.Application.Statements.Queries
{
    public class GetStatementQuery : IRequest<Statement>
    {
        public Guid StatementId { get; set; }
        public bool IncludeAttachments { get; set; }
        public ResultFormat Format { get; set; }

        public static GetStatementQuery Create(Guid statementId, bool includeAttachments = false, ResultFormat format = ResultFormat.Exact)
        {
            return new GetStatementQuery()
            {
                StatementId = statementId,
                IncludeAttachments = includeAttachments,
                Format = format
            };
        }
    }
}
