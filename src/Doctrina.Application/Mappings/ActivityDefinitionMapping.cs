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
            configuration.CreateMap<ActivityDefinition, ActivityDefinitionEntity>()
                .ForMember(ent => ent.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(ent => ent.Extensions, opt => opt.MapFrom(x => x.Extentions))
                .ForMember(ent => ent.MoreInfo, opt => opt.MapFrom(x => x.MoreInfo.ToString()))
                .ForMember(ent => ent.Name, opt => opt.MapFrom(x => x.Name));
        }
    }
}
