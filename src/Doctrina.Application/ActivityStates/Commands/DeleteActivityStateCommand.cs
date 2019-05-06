using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Documents;
using Doctrina.Domain.Entities.Extensions;
using Doctrina.Persistence;
using Doctrina.xAPI;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctrina.Application.ActivityStates.Commands
{
    public class DeleteActivityStateCommand : IRequest
    {
        public Iri ActivityId { get; set; }
        public Agent Agent { get; set; }
        public Guid? Registration { get; set; }

        public class Handler : IRequestHandler<DeleteActivityStateCommand>
        {
            private DoctrinaDbContext _context;
            private IMapper _mapper;

            public Handler(DoctrinaDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(DeleteActivityStateCommand request, CancellationToken cancellationToken)
            {

                string activityHash = request.ActivityId.ComputeHash();
                AgentEntity agent = _mapper.Map<AgentEntity>(request.Agent);

                var query = _context.ActivityStates
                    .Include(x => x.Document)
                    .Where(x => x.ActivityEntityId == activityHash)
                    .WhereAgent(agent);

                if (request.Registration.HasValue)
                {
                    query.Where(x => x.Registration == request.Registration);
                }

                ActivityStateEntity entity = await query.SingleOrDefaultAsync();

                _context.ActivityStates.Remove(entity);

                return await Unit.Task;
            }
        }
    }
}
