using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.DataTypes;
using Doctrina.Persistence;
using Doctrina.xAPI.Helpers;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doctrina.Application.Verbs.Commands
{
    public class MergeVerbCommand : IRequest<VerbEntity>
    {
        public Iri Id { get; set; }

        public LanguageMap Display { get; set; }

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

                var curr = _context.Verbs.SingleOrDefault(x => x.VerbId == verbId);
                if (curr != null)
                {
                    // Update canonical data
                    //var jsonString = curr.Display;
                    //var langMaps = JsonConvert.DeserializeObject<LanguageMap>(jsonString);
                    // TODO: Update maps
                }
                else
                {
                    curr = new VerbEntity()
                    {
                        VerbId = verbId,
                        Id = request.Id.ToString(),
                        Display = request.Display.,
                    };
                    _context.Verbs.Add(curr);
                }

                return curr;
            }
        }
    }
}
