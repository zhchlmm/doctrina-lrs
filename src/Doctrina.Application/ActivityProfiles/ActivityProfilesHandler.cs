using Doctrina.Application.Activities.Commands;
using Doctrina.Application.ActivityProfiles.Commands;
using Doctrina.Application.ActivityProfiles.Queries;
using Doctrina.Domain.Entities.Documents;
using Doctrina.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.ActivityProfiles
{
    public class ActivityProfilesHandler :
        IRequestHandler<CreateActivityProfileCommand, ActivityProfileEntity>,
        IRequestHandler<UpdateActivityProfileCommand>,
        IRequestHandler<GetActivityProfileQuery, ActivityProfileEntity>,
        IRequestHandler<GetActivityProfilesQuery, ICollection<ActivityProfileEntity>>,
        IRequestHandler<DeleteActivityProfileCommand>
    {
        private readonly DoctrinaDbContext _context;
        private readonly IMediator _mediator;

        public ActivityProfilesHandler(DoctrinaDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateActivityProfileCommand request, CancellationToken cancellationToken)
        {
            ActivityProfileEntity profile = await Handle(new GetActivityProfileQuery()
            {
                ActivityId = request.ActivityId,
                ProfileId = request.ProfileId,
                Registration = request.Registration
            }, cancellationToken);

            profile.UpdateDate = DateTime.UtcNow;
            profile.Document.Update(request.Content, request.ContentType);

            _context.ActivityProfiles.Update(profile);

            return await Unit.Task;
        }

        public async Task<ActivityProfileEntity> Handle(CreateActivityProfileCommand request, CancellationToken cancellationToken)
        {
            var activity = await _mediator.Send(new MergeActivityIriCommand()
            {
                ActivityId = request.ActivityId
            });

            var profile = new ActivityProfileEntity()
            {
                Key = Guid.NewGuid(),
                ActivityEntityId = activity.ActivityEntityId,
                ProfileId = request.ProfileId,
                RegistrationId = request.Registration,
                Document = DocumentEntity.Create(request.Content, request.ContentType)
            };

            _context.ActivityProfiles.Add(profile);
            _context.SaveChanges();

            //return profile;
            return profile;
        }

        public async Task<ActivityProfileEntity> Handle(GetActivityProfileQuery request, CancellationToken cancellationToken)
        {
            string activityHash = request.ActivityId.ComputeHash();
            ActivityProfileEntity profile = await _context.ActivityProfiles.FirstOrDefaultAsync(x => 
                x.ActivityEntityId == activityHash && 
                x.ProfileId == request.ProfileId &&
                x.RegistrationId == request.Registration
            );

            return profile;
        }

        public async Task<ICollection<ActivityProfileEntity>> Handle(GetActivityProfilesQuery request, CancellationToken cancellationToken)
        {
            var activityHash = request.ActivityId.ComputeHash();
            var query = _context.ActivityProfiles.Where(x => x.ActivityEntityId == activityHash);
            if (request.Since.HasValue)
            {
                query = query.Where(x => x.UpdateDate >= request.Since);
            }
            query = query.OrderByDescending(x => x.UpdateDate);
            return await query.ToListAsync();
        }

        public async Task<Unit> Handle(DeleteActivityProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await Handle(new GetActivityProfileQuery()
            {
                ActivityId = request.ActivityId,
                ProfileId = request.ProfileId,
                Registration = request.Registration
            }, cancellationToken);

            _context.ActivityProfiles.Remove(profile);

            return await Unit.Task;
        }
    }
}
