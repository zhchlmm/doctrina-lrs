using AutoMapper;
using Doctrina.Application.Interfaces.Mapping;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;

namespace Doctrina.Application.Mappings
{
    public class AgentsMappings : IHaveCustomMapping
    {
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Agent, AgentEntity>()
                .Include<Group, GroupEntity>()
                .ForMember(ent => ent.ObjectType, opt => opt.MapFrom(x => x.ObjectType))
                .ForMember(ent => ent.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(ent => ent.Mbox, opt => opt.MapFrom(x => x.Mbox.ToString()))
                .ForMember(ent => ent.Mbox_SHA1SUM, opt => opt.MapFrom(x => x.Mbox_SHA1SUM))
                .ForMember(ent => ent.OpenId, opt => opt.MapFrom(x => x.OpenId))
                .ForMember(ent => ent.Account, opt => opt.MapFrom(x => x.Account))
                .AfterMap((source, dist) => { dist.AgentHash = dist.ComputeHash(); });

            configuration.CreateMap<Group, GroupEntity>()
               .ForMember(ent => ent.Members, opt => opt.MapFrom(x => x.Member));

            configuration.CreateMap<Domain.Entities.Account, xAPI.Account>()
                .ForMember(ent => ent.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(ent => ent.HomePage, opt => opt.MapFrom(x => x.HomePage));
        }
    }
}
