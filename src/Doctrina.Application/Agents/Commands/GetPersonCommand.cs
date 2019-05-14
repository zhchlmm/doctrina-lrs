using AutoMapper;
using Doctrina.Domain.Entities;
using Doctrina.Persistence;
using Doctrina.xAPI;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Agents.Commands
{
    public class GetPersonCommand : IRequest<Person>
    {
        public Agent Agent { get; set; }

        public class Handler : IRequestHandler<GetPersonCommand, Person>
        {
            private readonly DoctrinaDbContext _context;
            private readonly IMapper _mapper;

            public Handler(DoctrinaDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Person> HandleAsync(GetPersonCommand request, CancellationToken cancellationToken)
            {
                var person = new Person();
                person.Add(request.Agent);

                var agentEntity = _mapper.Map<AgentEntity>(request.Agent);

                agentEntity = await _context.Agents.FirstOrDefaultAsync(x => x.AgentEntityId == agentEntity.AgentEntityId);

                if (agentEntity != null)
                {
                    person.Add(_mapper.Map<Agent>(agentEntity));
                }

                return person;
            }
        }
    }
}
