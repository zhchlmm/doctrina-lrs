using Doctrina.xAPI;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.About.Queries
{
    public class GetAboutQuery : IRequest<xAPI.About>
    {
        public class Handler : IRequestHandler<GetAboutQuery, xAPI.About>
        {
            public Task<xAPI.About> Handle(GetAboutQuery request, CancellationToken cancellationToken)
            {
                var about = new xAPI.About()
                {
                    Version = ApiVersion.GetSupported().Select(x => x.Key)
                };

                return Task.FromResult(about);
            }
        }
    }
}
