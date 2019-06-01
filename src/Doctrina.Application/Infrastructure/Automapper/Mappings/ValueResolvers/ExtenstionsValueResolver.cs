using AutoMapper;
using Doctrina.Domain.Entities.OwnedTypes;

namespace Doctrina.Application.Mappings.ValueResolvers
{
    public class ExtenstionsValueResolver :
        IMemberValueResolver<object, object, ExtensionsCollection, xAPI.Extensions>,
        IMemberValueResolver<object, object, xAPI.Extensions, ExtensionsCollection>
    {
        public ExtensionsCollection Resolve(object source, object destination, xAPI.Extensions sourceMember, ExtensionsCollection destMember, ResolutionContext context)
        {
            if(sourceMember == null)
            {
                return null;
            }

            var collection = new ExtensionsCollection();

            foreach (var p in sourceMember)
            {
                collection.Add(p.Key, p.Value);
            }
            return collection;
        }

        public xAPI.Extensions Resolve(object source, object destination, ExtensionsCollection sourceMember, xAPI.Extensions destMember, ResolutionContext context)
        {
            var ext = new xAPI.Extensions();
            foreach(var mem in sourceMember)
            {
                ext.Add(mem);
            }
            return ext;
        }
    }
}
