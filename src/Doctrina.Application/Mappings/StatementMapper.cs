using AutoMapper;
using Doctrina.Application.Interfaces.Mapping;
using Doctrina.Application.Mappings.ValueResolvers;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.Application.Mappings
{
    //public class StatementMapper : 
    //    IMapFrom<Statement, StatementEntity>,
    //    IMapFrom<StatementEntity, Statement>
    //{
    //    private IMapperContext _mapper;

    //    public StatementMapper(IMapperContext mapper)
    //    {
    //        _mapper = mapper;
    //    }

    //    public StatementEntity MapFrom(Statement target, StatementEntity source)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Statement MapFrom(StatementEntity source, Statement target)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class StatementBaseMapper :
    //    IMapFrom<StatementBase, StatementBaseEntity>,
    //    IMapFrom<StatementBaseEntity, StatementBase>
    //{
    //    private IMapperContext _mapper;

    //    public StatementBaseMapper(IMapperContext mapper)
    //    {
    //        _mapper = mapper;
    //    }

    //    public StatementBaseEntity MapFrom(StatementBase source, StatementBaseEntity target)
    //    {
    //        target.Actor = _mapper.MapFrom<AgentEntity>(source.Actor);
    //        target.Verb = _mapper.MapFrom<VerbEntity>(source.Verb);

    //        //foreach(var attachment in )
    //        //target.Attachments = _mapper.MapFrom<ICollection<AttachmentEntity>>(source.Attachments);

    //        return target;
    //    }

    //    public StatementBase MapFrom(StatementBaseEntity source, StatementBase target)
    //    {
    //        target.Actor = _mapper.MapFrom<Agent>(source.Actor);

    //        return target;
    //    }
    //}

    public class StatementMapping : IHaveCustomMapping
    {
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<StatementObjectBase, StatementObjectEntity>()
               .ForMember(ent => ent.ObjectType, opt => opt.MapFrom(x => (EntityObjectType)Enum.Parse(typeof(EntityObjectType), (string)x.ObjectType)))
               .ReverseMap();


            configuration.CreateMap<Agent, AgentEntity>()
                .ForMember(e => e.ObjectType, opt => opt.Ignore())
               .ForMember(ent => ent.Name, opt => opt.MapFrom(x => x.Name))
               .ForMember(ent => ent.Mbox, opt => opt.MapFrom(x => x.Mbox.ToString()))
               .ForMember(ent => ent.Mbox_SHA1SUM, opt => opt.MapFrom(x => x.Mbox_SHA1SUM))
               .ForMember(ent => ent.OpenId, opt => opt.MapFrom(x => x.OpenId.ToString()))
               .ForMember(ent => ent.Account, opt => opt.MapFrom(x => x.Account))
               .ForMember(ent => ent.AgentHash, opt => opt.Ignore())
               .ReverseMap();

            configuration.CreateMap<Group, GroupEntity>()
               .IncludeBase<Agent, AgentEntity>()
               .ForMember(ent => ent.Members, opt => opt.MapFrom(x => x.Member))
               .ReverseMap();

            configuration.CreateMap<xAPI.Account, Domain.Entities.Account>()
                .ForMember(ent => ent.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(ent => ent.HomePage, opt => opt.MapFrom(x => x.HomePage.ToString()))
                .ReverseMap();

            //configuration.CreateMap<StatementBase, StatementBaseEntity>()
            //    // Ignore potential mapped props
            //    //.IncludeBase<StatementObjectBase, StatementObjectBaseEntity>()
            //    //.ForMember(ent => ent.ObjectType, opt => opt.MapFrom(x => (EntityObjectType)Enum.Parse(typeof(EntityObjectType), (string)x.ObjectType)))
            //    .ForMember(x => x.StatementBaseId, opt => opt.Ignore())
            //    .ForMember(x => x.Actor, opt => opt.MapFrom(x => x.Actor))
            //    .ForMember(x => x.Verb, opt => opt.MapFrom(x => x.Verb))
            //    .ForMember(x => x.Object, opt => opt.MapFrom<ObjectValueResolver, StatementObjectBase>(x => x.Object))
            //    .ForMember(x => x.Result, opt => opt.MapFrom(x => x.Result))
            //    .ForMember(x => x.Context, opt => opt.MapFrom(x => x.Context))
            //    .ForMember(x => x.Timestamp, opt => opt.MapFrom(x => x.Timestamp))
            //    .ForMember(x => x.Attachments, opt => opt.MapFrom(x => x.Attachments))
            //    .ReverseMap();

            configuration.CreateMap<Statement, StatementEntity>()
                // Statement base
                .ForMember(x => x.StatementId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Actor, opt => opt.MapFrom(x => x.Actor))
                .ForMember(x => x.Verb, opt => opt.MapFrom(x => x.Verb))
                .ForMember(x => x.Object, opt => opt.MapFrom<ObjectValueResolver, IStatementObject>(x => x.Object))
                .ForMember(x => x.Timestamp, opt => opt.MapFrom(x => x.Timestamp))
                .ForMember(x => x.Attachments, opt => opt.MapFrom(x => x.Attachments))
                // Statement only
                .ForMember(x => x.Result, opt => opt.MapFrom(x => x.Result))
                .ForMember(x => x.Context, opt => opt.MapFrom(x => x.Context))
                .ForMember(x => x.Authority, opt => opt.MapFrom(x => x.Authority))
                .ForMember(x => x.Stored, opt => opt.MapFrom(x => x.Stored))
                .ForMember(x => x.Version, opt => opt.MapFrom(x => x.Version))
                // Database specfic
                .ForMember(x => x.AuthorityId, opt => opt.Ignore())
                .ForMember(x => x.Voided, opt => opt.Ignore())
                .ForMember(x => x.FullStatement, opt => opt.Ignore()/*MapFrom(x=> x.ToJson())*/)
                .ReverseMap();

            configuration.CreateMap<SubStatement, SubStatementEntity>()
                .ForMember(e=> e.ObjectType, opt => opt.Ignore())
                .ForMember(x => x.Actor, opt => opt.MapFrom(x => x.Actor))
                .ForMember(x => x.Verb, opt => opt.MapFrom(x => x.Verb))
                .ForMember(x => x.Object, opt => opt.MapFrom<ObjectValueResolver, IStatementObject>(x => x.Object))
                .ForMember(x => x.Result, opt => opt.MapFrom(x => x.Result))
                .ForMember(x => x.Context, opt => opt.MapFrom(x => x.Context))
                .ForMember(x => x.Timestamp, opt => opt.MapFrom(x => x.Timestamp))
                .ForMember(x => x.Attachments, opt => opt.MapFrom(x => x.Attachments))
                .ReverseMap();

            configuration.CreateMap<StatementRef, StatementRefEntity>()
                .ForMember(e => e.ObjectType, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id));

            configuration.CreateMap<Verb, VerbEntity>()
                .ForMember(x => x.VerbHash, opt => opt.MapFrom(x => x.Id.ComputeHash()))
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(x => x.Display, opt => opt.MapFrom<LanguageMapValueResolver, LanguageMap>(x => x.Display))
                .ReverseMap();

            configuration.CreateMap<Activity, ActivityEntity>()
                .ForMember(e => e.ObjectType, opt => opt.Ignore())
                .ForMember(e => e.ActivityId, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(e => e.ActivityHash, opt => opt.MapFrom(x => x.Id.ComputeHash()))
                .ForMember(entity => entity.Definition, opt => opt.MapFrom(x => x.Definition))
                //.ForMember(entity => entity.Definition, opt => opt.Ignore())
                .ReverseMap();

            configuration.CreateMap<ActivityDefinition, ActivityDefinitionEntity>()
                .ForMember(ent => ent.ActivityDefinitionId, opt => opt.Ignore())
                .ForMember(ent => ent.Type, opt => opt.MapFrom(x => x.Type.ToString()))
                //.ForMember(ent => ent.ActivityHash, opt => opt.Ignore())
                //.ForMember(ent => ent.Activity, opt => opt.Ignore())
                .ForMember(ent => ent.Descriptions, opt => opt.MapFrom<LanguageMapValueResolver, LanguageMap>(x => x.Description))
                .ForMember(ent => ent.Extensions, opt => opt.MapFrom<ExtenstionsValueResolver, xAPI.Extensions>(x => x.Extentions))
                .ForMember(ent => ent.MoreInfo, opt => opt.MapFrom(x => x.MoreInfo.ToString()))
                .ForMember(ent => ent.Names, opt => opt.MapFrom<LanguageMapValueResolver, LanguageMap>(src => src.Name))
                .ReverseMap();

            configuration.CreateMap<Result, ResultEntity>()
                //.ForMember(x => x.ResultId, opt => opt.Ignore())
                .ForMember(x => x.Completion, opt => opt.MapFrom(x => x.Completion))
                .ForMember(x => x.Score, opt => opt.MapFrom(x => x.Score))
                .ForMember(x => x.Duration, opt => opt.Ignore()/*MapFrom(x => (string)x.Duration.ToString())*/)
                .ForMember(x => x.Response, opt => opt.MapFrom(x => x.Response))
                .ForMember(x => x.Success, opt => opt.MapFrom(x => x.Success))
                .ForMember(x => x.Extensions, opt => opt.MapFrom<ExtenstionsValueResolver, xAPI.Extensions>(x => x.Extentions))
                .ReverseMap();

            configuration.CreateMap<Score, ScoreEntity>()
                .ReverseMap();

            configuration.CreateMap<Context, ContextEntity>()
                .ForMember(x => x.ContextId, opt => opt.Ignore())
                .ForMember(x => x.ContextActivities, opt => opt.MapFrom(x => x.ContextActivities))
                .ReverseMap();

            configuration.CreateMap<Attachment, AttachmentEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.ContentType, conf => conf.MapFrom(p => p.ContentType))
                .ForMember(x => x.FileUrl, conf => conf.MapFrom(p => p.FileUrl.ToString()))
                .ForMember(x => x.Length, opt => opt.MapFrom(p => p.Length))
                .ForMember(x => x.Display, opt => opt.MapFrom<LanguageMapValueResolver, LanguageMap>(p => p.Display))
                .ForMember(x => x.Description, opt => opt.MapFrom<LanguageMapValueResolver, LanguageMap>(p => p.Description))
                .ReverseMap();

            configuration.CreateMap<ContextActivities, ContextActivitiesEntity>()
                .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Category))
                .ForMember(x => x.Parent, opt => opt.MapFrom(x => x.Parent))
                .ForMember(x => x.Grouping, opt => opt.MapFrom(x => x.Grouping))
                .ForMember(x => x.Other, opt => opt.MapFrom(x => x.Other));

            configuration.CreateMap<ActivityCollection, ContextActivityCollection>();
        }

    }
}
