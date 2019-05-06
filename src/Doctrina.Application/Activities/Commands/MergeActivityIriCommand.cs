using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Application.Activities.Commands
{
    public class MergeActivityIriCommand : IRequest<ActivityEntity>
    {
        public Iri ActivityId { get; set; }
    }
}
