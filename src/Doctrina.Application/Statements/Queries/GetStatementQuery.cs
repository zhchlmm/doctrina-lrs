using MediatR;
using System.Collections.Generic;

namespace Doctrina.Application.Statements.Queries
{
    public class GetStatementQuery : IRequest<List<CategoryPreviewDto>>
    {
    }
}
