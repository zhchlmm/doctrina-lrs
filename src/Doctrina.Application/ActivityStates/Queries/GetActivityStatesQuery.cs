﻿using Doctrina.xAPI;
using Doctrina.xAPI.Documents;
using MediatR;
using System;
using System.Collections.Generic;

namespace Doctrina.Application.ActivityStates.Queries
{
    public class GetActivityStatesQuery : IRequest<ICollection<ActivityStateDocument>>
    {
        public Iri ActivityId { get; set; }
        public Agent Agent { get; set; }
        public Guid? Registration { get; set; }
        public DateTime? Since { get; set; }
    }
}
