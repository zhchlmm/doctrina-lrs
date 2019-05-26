using AutoMapper;
using Doctrina.Domain.Entities.OwnedTypes;
using Doctrina.xAPI;

namespace Doctrina.Application.Mappings.ValueResolvers
{
    public class LanguageMapValueResolver :
        IMemberValueResolver<object, object, LanguageMap, LanguageMapCollection>,
        IMemberValueResolver<object, object, LanguageMapCollection, LanguageMap>
    {
        public LanguageMapCollection Resolve(object source, object destination, LanguageMap sourceMember, LanguageMapCollection destMember, ResolutionContext context)
        {
            var collection = new LanguageMapCollection();

            foreach (var p in sourceMember)
            {
                collection.Add(p);
            }
            return collection;
        }

        public LanguageMap Resolve(object source, object destination, LanguageMapCollection sourceMember, LanguageMap destMember, ResolutionContext context)
        {
            var map = new xAPI.LanguageMap();
            foreach(var mem in sourceMember)
            {
                map.Add(mem);
            }

            return map;
        }
    }
}
