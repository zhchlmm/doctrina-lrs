﻿using AutoMapper;
using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.OwnedTypes;
using Doctrina.xAPI;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Application.Activities.Commands
{
    public class MergeActivityEntityCommand : IRequest<ActivityEntity>
    {
        
    }
}
