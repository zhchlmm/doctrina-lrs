using AutoMapper;
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
        public void CreateMappings(Profile configuration)
        {
            // TODO: Move mappings
            configuration.CreateMap<Activity, ActivityEntity>()
                .ForMember(entity => entity.ActivityId, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(entity => entity.ActivityHash, opt => opt.MapFrom(x => x.Id.ComputeHash()))
                .ForMember(entity => entity.Definition, opt => opt.MapFrom(x => x.Definition));

            configuration.CreateMap<ActivityDefinition, ActivityDefinitionEntity>()
                .ForMember(ent => ent.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(ent => ent.Extensions, opt => opt.MapFrom(x => x.Extentions))
                .ForMember(ent => ent.MoreInfo, opt => opt.MapFrom(x => x.MoreInfo.ToString()))
                .ForMember(ent => ent.Name, opt => opt.MapFrom(x => x.Name));

            configuration.CreateMap<LanguageMap, ICollection<LanguageMapEntity>>()
                .ConstructUsing(x => x.Select(p => new LanguageMapEntity() { LanguageCode = p.Key, Description = p.Value }).ToList())
                .ReverseMap()
                .ConstructUsing((collection, context) =>
                {
                    var map = new LanguageMap();

                    foreach (var item in collection)
                    {
                        map.Add(item.LanguageCode, item.Description);
                    }

                    return map;
                });

            configuration.CreateMap<xAPI.Extensions, ICollection<ExtensionEntity>>()
                .ConstructUsing(x => x.Select(p => new ExtensionEntity() { Key = p.Key.ToString(), Value = p.Value.ToString() }).ToList())
                .ReverseMap()
                .ConstructUsing((collection, context) =>
                {
                    var extensions = new Extensions();

                    foreach (var item in collection)
                    {
                        extensions.Add(new System.Uri(item.Key), Newtonsoft.Json.Linq.JToken.Parse(item.Value));
                    }

                    return extensions;
                });
        }
    }
}
