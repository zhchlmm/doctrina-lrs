using AutoMapper;
using Doctrina.Application.Interfaces.Mapping;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;

namespace Doctrina.Application.Mappings
{
    public class ActivityConfiguraiton : IHaveCustomMapping
    {
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Activity, ActivityEntity>()
                .ForMember(entity => entity.ActivityId, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(entity => entity.ActivityEntityId, opt => opt.MapFrom(x => x.Id.ComputeHash()))
                .ForMember(entity => entity.Definition, opt => opt.MapFrom(x => x.Definition));
        }
    }
}
