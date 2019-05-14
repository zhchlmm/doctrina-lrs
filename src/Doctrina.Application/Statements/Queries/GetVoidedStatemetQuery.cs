using Doctrina.xAPI;
using MediatR;
using System;

namespace Doctrina.Application.Statements.Queries
{
    public class GetVoidedStatemetQuery : IRequest<Statement>
    {
        public ResultFormat Format { get; private set; }
        public Guid VoidedStatementId { get; private set; }
        public bool IncludeAttachments { get; private set; }

        public static GetVoidedStatemetQuery Create(Guid voidedStatementId, bool includeAttachments, ResultFormat format)
        {
            return new GetVoidedStatemetQuery()
            {
                VoidedStatementId = voidedStatementId,
                IncludeAttachments = includeAttachments,
                Format = format
            };
        }
    }
}
