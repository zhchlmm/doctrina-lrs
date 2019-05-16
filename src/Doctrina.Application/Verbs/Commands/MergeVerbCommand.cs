using Doctrina.Application.Interfaces;
using Doctrina.Domain.Entities;
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
            private readonly IDoctrinaDbContext _context;
            private readonly IMediator _mediator;

            public Handler(IDoctrinaDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<VerbEntity> Handle(MergeVerbCommand request, CancellationToken cancellationToken)
            {
                string verbHash = Iri.ComputeHash(request.Verb.Id);

                var verb = await _context.Verbs.FindAsync(request.Verb.Id);
                if (verb != null)
                {
                    // TODO: Update verb Display language maps
                    foreach (var dis in request.Verb.Display)
                    {
                        verb.Display.Add(dis);
                    }
                }
                else
                {
                    verb = request.Verb;
                    verb.VerbHash = verbHash;

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
