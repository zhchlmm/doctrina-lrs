using Doctrina.Domain.Entities;
using Doctrina.Persistence;
using Doctrina.xAPI;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Verbs.Commands
{
    public class MergeVerbCommand : IRequest<VerbEntity>
    {
        public VerbEntity Verb { get; set; }

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
                string verbChecksum = Iri.ComputeHash(request.Verb.Id);

                var verb = await _context.Verbs.FindAsync(verbChecksum);
                if (verb != null)
                {
                    // TODO: Update verb Display language maps
                    foreach(var dis in request.Verb.Display)
                    {
                        verb.Display.Add(dis);
                    }
                }
                else
                {
                    verb = request.Verb;
                    verb.Checksum = verbChecksum;

                    _context.Verbs.Add(verb);
                }

                return verb;
            }
        }

        internal static MergeVerbCommand Create(VerbEntity verb)
        {
            return new MergeVerbCommand()
            {
                Verb = verb
            };
        }
    }
}
