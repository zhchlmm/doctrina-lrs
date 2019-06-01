using Doctrina.xAPI;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        public class Handler :IRequestHandler<GetVoidedStatemetQuery, Statement>
        {
            public Task<Statement> Handle(GetVoidedStatemetQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
