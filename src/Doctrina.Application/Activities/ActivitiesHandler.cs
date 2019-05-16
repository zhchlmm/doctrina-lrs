using AutoMapper;
using Doctrina.Application.Activities.Queries;
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

namespace Doctrina.Application.Activities
{
    public class ActivitiesHandler :
        IRequestHandler<GetActivityQuery, Activity>
    {
        private readonly DoctrinaDbContext _context;
        private readonly IMapper _mapper;

        public ActivitiesHandler(DoctrinaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Activity> Handle(GetActivityQuery request, CancellationToken cancellationToken)
        {
            string hash = request.ActivityId.ComputeHash();
            var activity = await _context.Activities.FirstOrDefaultAsync(x => x.ActivityHash == hash);
            return _mapper.Map<Activity>(activity);
        }
    }
}
