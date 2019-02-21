using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.DataTypes;
using Doctrina.Persistence;
using Doctrina.xAPI;
using Doctrina.xAPI.Helpers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Verbs.Commands
{
    public class MergeVerbCommand : IRequest<VerbEntity>
    {
        public xAPI.Iri Id { get; set; }

        public xAPI.LanguageMap Display { get; set; }

        public class Handler : IRequestHandler<MergeVerbCommand, VerbEntity>
        {
            private readonly DoctrinaDbContext _context;
            private readonly IMediator _mediator;

            public Handler(
                DoctrinaDbContext context,
                IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<VerbEntity> Handle(MergeVerbCommand request, CancellationToken cancellationToken)
            {
                string verbId = SHAHelper.ComputeHash(request.Id.ToString());

                var verb = await _context.Verbs.FindAsync(verbId);
                if (verb != null)
                {
                    // TODO: Update verb Display language maps
                }
                else
                {
                    verb = new VerbEntity()
                    {
                        VerbId = verbId,
                        Id = request.Id.ToString(),
                        //Display = request.Display.,
                    };

                    _context.Verbs.Add(verb);
                }

                return verb;
            }
        }
    }
}
