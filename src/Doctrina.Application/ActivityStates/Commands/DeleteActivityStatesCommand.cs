using AutoMapper;
using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Extensions;
using Doctrina.Persistence;
using Doctrina.xAPI;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.ActivityStates.Commands
{
    public class DeleteActivityStatesCommand : IRequest
    {
        public Iri ActivityId { get; set; }
        public Agent Agent { get; set; }
        public Guid? Registration { get; set; }

        public class Handler : IRequestHandler<DeleteActivityStatesCommand>
        {
            private DoctrinaDbContext _context;
            private IMapper _mapper;

            public Handler(DoctrinaDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(DeleteActivityStatesCommand request, CancellationToken cancellationToken)
            {
                string activityChecksum = request.ActivityId.ComputeHash();
                AgentEntity agent = _mapper.Map<AgentEntity>(request.Agent);

                var sql = _context.ActivityStates.WhereAgent(agent);

                sql.Where(x => x.ActivityEntityId == activityChecksum);

                if (request.Registration.HasValue)
                {
                    sql.Where(x => x.Registration == request.Registration);
                }

                _context.ActivityStates.RemoveRange(sql);
                _context.SaveChanges();

                return await Unit.Task;
            }
        }
    }
}
