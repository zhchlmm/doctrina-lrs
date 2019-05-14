using AutoMapper;
using Doctrina.Application.Interfaces.Mapping;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;

namespace Doctrina.Application.Mappings
{
    public class StatementMapping : IHaveCustomMapping
    {
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<StatementBase, StatementBaseEntity>()
                .ForMember(x => x.Actor, opt => opt.MapFrom(x => x.Actor))
                .ForMember(x => x.Verb, opt => opt.MapFrom(x => x.Verb))
                .ForMember(x => x.Object, opt => opt.MapFrom(x => x.Object))
                .ForMember(x => x.Result, opt => opt.MapFrom(x => x.Result))
                .ForMember(x => x.Context, opt => opt.MapFrom(x => x.Context))
                .ForMember(x => x.Timestamp, opt => opt.MapFrom(x => x.Timestamp))
                .ForMember(x => x.Attachments, opt => opt.MapFrom(x => x.Attachments));

            configuration.CreateMap<Statement, StatementEntity>()
                .ForMember(x => x.StatementId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Stored, opt => opt.MapFrom(x => x.Stored))
                .ForMember(x => x.Version, opt => opt.MapFrom(x => x.Version))
                .ForMember(x => x.Authority, opt => opt.MapFrom(x => x.Authority))
                .IncludeBase<StatementBase, StatementBaseEntity>();

            configuration.CreateMap<SubStatement, SubStatementEntity>()
                //.ForMember(x => x.SubStatementId, opt => opt.MapFrom(x => x.Sub))
                .IncludeBase<StatementBase, StatementBaseEntity>();
        }
    }
}
