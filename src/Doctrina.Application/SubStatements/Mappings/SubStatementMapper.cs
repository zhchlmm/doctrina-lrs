using AutoMapper;
using Doctrina.Application.Interfaces.Mapping;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Application.SubStatements.Mappings
{
    public class SubStatementMapper : IHaveCustomMapping
    {
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<SubStatement, SubStatementEntity>()
                .ForMember(x => x.Result, opt => opt.MapFrom(x => x.Result))
                .ForMember(x=> x.Actor, opt => opt.MapFrom(x=> x.Actor))
                .ForMember(x=> x.ObjectType, opt => opt.MapFrom(x=> x.ObjectType))
                .ForMember(x=> x.Timestamp, opt => opt.MapFrom(x=> x.Timestamp));
        }
    }
}
