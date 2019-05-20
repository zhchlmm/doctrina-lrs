using AutoMapper;
using Doctrina.Application.Interfaces.Mapping;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;

namespace Doctrina.Application.Mappings
{
    public class ActivityDefinitionMapping : IHaveCustomMapping
    {
        public void CreateMappings(Profile configuration)
        {
            //configuration.CreateMap<ActivityDefinition, ActivityDefinitionEntity>()
            //    .ForMember(ent => ent.Descriptions, opt => opt.MapFrom(x => x.Description))
            //    .ForMember(ent => ent.Extensions, opt => opt.MapFrom(x => x.Extentions))
            //    .ForMember(ent => ent.MoreInfo, opt => opt.MapFrom(x => x.MoreInfo.ToString()))
            //    .ForMember(ent => ent.Names, opt => opt.MapFrom(x => x.Name))
            //    .ForMember(ent => ent.ActivityDefinitionId, opt => opt.Ignore())
            //    .ForMember(ent => ent.ActivityHash, opt => opt.Ignore())
            //    .ForMember(ent => ent.Activity, opt => opt.Ignore());
        }
    }
}
