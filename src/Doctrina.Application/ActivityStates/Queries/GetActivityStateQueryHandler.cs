using AutoMapper;
using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Documents;
using Doctrina.Domain.Entities.Extensions;
using Doctrina.Persistence;
using Doctrina.xAPI.Documents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.ActivityStates.Queries
{
    public class GetActivityStateQueryHandler : IRequestHandler<GetActivityStateQuery, ActivityStateDocument>
    {
        private readonly DoctrinaDbContext _context;
        private readonly IMapper _mapper;

        public GetActivityStateQueryHandler(DoctrinaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActivityStateDocument> Handle(GetActivityStateQuery request, CancellationToken cancellationToken)
        {
            string activityHash = request.ActivityId.ComputeHash();

            AgentEntity agent = _mapper.Map<AgentEntity>(request.Agent);

            var query = _context.ActivityStates
                .Include(x => x.Document)
                .Where(x=> x.ActivityEntityId == activityHash)
                .WhereAgent(agent);

            if (request.Registration.HasValue)
            {
                query.Where(x => x.Registration == request.Registration);
            }

            ActivityStateEntity entity = await query.SingleOrDefaultAsync();

            return _mapper.Map(entity, new ActivityStateDocument());
        }
    }
}
