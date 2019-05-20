using AutoMapper;
using Doctrina.Application.Interfaces.Mapping;
using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.OwnedTypes;
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
            configuration.CreateMap<Agent, AgentEntity>()
               .ForMember(ent => ent.ObjectType, opt => opt.MapFrom(x => (EntityObjectType)Enum.Parse(typeof(EntityObjectType), (string)x.ObjectType)))
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

            configuration.CreateMap<StatementBase, StatementBaseEntity>()
                // Ignore potential mapped props
                .ForMember(x => x.ObjectStatementRefId, opt => opt.MapFrom(x => x.Object is StatementRef ? ((StatementRef)x.Object).Id : (Guid?)null))
                .ForMember(x => x.ObjectAgent, opt => opt.MapFrom(x => x.Object is Agent ?
                    (Agent)x.Object : x.Object is Group ?
                    (Group)x.Object : null))
                .ForMember(x => x.ObjectActivity, opt => opt.MapFrom(x => (Activity)x.Object is Activity ? (Activity)x.Object : null))
                .ForMember(x => x.StatementId, opt => opt.Ignore())
                .ForMember(x => x.Actor, opt => opt.MapFrom(x => x.Actor))
                .ForMember(x => x.Verb, opt => opt.MapFrom(x => x.Verb))
                .ForMember(x => x.Object, opt => opt.MapFrom(x => x.Object))
                .ForMember(x => x.Result, opt => opt.MapFrom(x => x.Result))
                .ForMember(x => x.Context, opt => opt.MapFrom(x => x.Context))
                .ForMember(x => x.Context, opt => opt.Ignore())
                .ForMember(x => x.Timestamp, opt => opt.MapFrom(x => x.Timestamp))
                .ForMember(x => x.Attachments, opt => opt.MapFrom(x => x.Attachments))
                .ReverseMap()
                .ForPath(x => x.Object.ObjectType, conf => conf.MapFrom(x => (ObjectType)Enum.GetName(typeof(EntityObjectType), x.ObjectObjectType)));

            configuration.CreateMap<Statement, StatementEntity>()
                .IncludeBase<StatementBase, StatementBaseEntity>()
                .ForMember(x => x.Authority, opt => opt.MapFrom(x => x.Authority))
                .ForMember(x => x.AuthorityId, opt => opt.Ignore())
                .ForMember(x => x.StatementId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Stored, opt => opt.MapFrom(x => x.Stored))
                .ForMember(x => x.Version, opt => opt.MapFrom(x => x.Version))
                .ForMember(x => x.Actor, opt => opt.MapFrom(x => x.Actor))
                .ForMember(x => x.Voided, opt => opt.Ignore())
                .ForMember(x => x.FullStatement, opt => opt.Ignore()/*MapFrom(x=> x.ToJson())*/)
                .ForMember(x => x.ObjectSubStatement, opt => opt.MapFrom(x => x.Object is SubStatement ? x.Object : null))
                .ForMember(x => x.ObjectSubStatementId, opt => opt.Ignore())
                .ForMember(x => x.ObjectObjectType, opt => opt.Ignore())
                .ReverseMap();

            configuration.CreateMap<SubStatement, SubStatementEntity>()
                .IncludeBase<StatementBase, StatementBaseEntity>()
                .ForMember(x => x.SubStatementId, opt => opt.Ignore())
                .ReverseMap();

            configuration.CreateMap<Verb, VerbEntity>()
                .ForMember(x => x.VerbHash, opt => opt.MapFrom(x => x.Id.ComputeHash()))
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(x => x.Display, opt => opt.MapFrom<LanguageMapValueResolver, LanguageMap>(x => x.Display))
                .ReverseMap();

            configuration.CreateMap<Activity, ActivityEntity>()
                .ForMember(e => e.ActivityId, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(e => e.ActivityHash, opt => opt.Ignore()/*MapFrom(x => x.Id.ComputeHash())*/)
                .ForMember(entity => entity.Definition, opt => opt.MapFrom(x => x.Definition))
                //.ForMember(entity => entity.Definition, opt => opt.Ignore())
                .ReverseMap();

            configuration.CreateMap<ActivityDefinition, ActivityDefinitionEntity>()
                .ForMember(ent => ent.ActivityDefinitionId, opt => opt.Ignore())
                .ForMember(ent => ent.ActivityHash, opt => opt.Ignore())
                .ForMember(ent => ent.Activity, opt => opt.Ignore())
                .ForMember(ent => ent.Descriptions, opt => opt.MapFrom<LanguageMapValueResolver, LanguageMap>(x => x.Description))
                .ForMember(ent => ent.Extensions, opt => opt.MapFrom<ExtenstionsValueResolver, xAPI.Extensions>(x => x.Extentions))
                .ForMember(ent => ent.MoreInfo, opt => opt.MapFrom(x => x.MoreInfo.ToString()))
                .ForMember(ent => ent.Names, opt => opt.MapFrom<LanguageMapValueResolver, LanguageMap>(src => src.Name))
                .ReverseMap();

            configuration.CreateMap<Result, ResultEntity>()
                .ForMember(x => x.ResultId, opt => opt.Ignore())
                .ForMember(x => x.StatementId, opt => opt.Ignore())
                .ForMember(x => x.Statement, opt => opt.Ignore())
                .ForMember(x => x.Completion, opt => opt.MapFrom(x => x.Completion))
                .ForMember(x => x.Score, opt => opt.MapFrom(x => x.Score))
                .ForMember(x => x.Duration, opt => opt.Ignore()/*MapFrom(x => (string)x.Duration.ToString())*/)
                .ForMember(x => x.Response, opt => opt.MapFrom(x => x.Response))
                .ForMember(x => x.Success, opt => opt.MapFrom(x => x.Success))
                .ForMember(x => x.Extensions, opt => opt.MapFrom<ExtenstionsValueResolver, xAPI.Extensions>(x => x.Extentions))
                .ReverseMap();

            configuration.CreateMap<Score, ScoreEntity>();

            configuration.CreateMap<Context, ContextEntity>()
                .ForMember(x => x.ContextId, opt => opt.Ignore())
                .ForMember(x => x.ContextActivities, opt => opt.MapFrom(x => x.ContextActivities))
                .ReverseMap();

            //configuration.CreateMap<ContextActivities, ContextActivitiesEntity>()
            //    .ForMember(x=> x.Category, opt=> opt.);

        }

    }

    public class LanguageMapValueResolver :
        IMemberValueResolver<object, object, LanguageMap, ICollection<LanguageMapEntity>>,
        IMemberValueResolver<object, object, ICollection<LanguageMapEntity>, LanguageMap>
    {
        public ICollection<LanguageMapEntity> Resolve(object source, object destination, LanguageMap sourceMember, ICollection<LanguageMapEntity> destMember, ResolutionContext context)
        {
            var collection = new HashSet<LanguageMapEntity>();

            foreach (var p in sourceMember)
            {
                collection.Add(new LanguageMapEntity() { LanguageCode = p.Key, Description = p.Value });
            }
            return collection;
        }

        public LanguageMap Resolve(object source, object destination, ICollection<LanguageMapEntity> sourceMember, LanguageMap destMember, ResolutionContext context)
        {
            var map = new LanguageMap();
            foreach(var mem in sourceMember)
            {
                map.Add(mem.LanguageCode, mem.Description);
            }

            return map;
        }
    }

    public class ExtenstionsValueResolver :
        IMemberValueResolver<object, object, ICollection<ExtensionEntity>, xAPI.Extensions>,
        IMemberValueResolver<object, object, xAPI.Extensions, ICollection<ExtensionEntity>>
    {
        public ICollection<ExtensionEntity> Resolve(object source, object destination, xAPI.Extensions sourceMember, ICollection<ExtensionEntity> destMember, ResolutionContext context)
        {
            var collection = new HashSet<ExtensionEntity>();

            foreach (var p in sourceMember)
            {
                collection.Add(new ExtensionEntity() { Key = p.Key.ToString(), Value = p.Value.ToString() });
            }
            return collection;
        }

        public xAPI.Extensions Resolve(object source, object destination, ICollection<ExtensionEntity> sourceMember, xAPI.Extensions destMember, ResolutionContext context)
        {
            var ext = new xAPI.Extensions();
            foreach(var mem in sourceMember)
            {
                ext.Add(new Uri(mem.Key), JToken.Parse(mem.Value));
            }
            return ext;
        }
    }
}
