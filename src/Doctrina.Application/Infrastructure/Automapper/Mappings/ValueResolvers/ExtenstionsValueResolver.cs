using AutoMapper;
using Doctrina.Domain.Entities.OwnedTypes;

namespace Doctrina.Application.Mappings.ValueResolvers
{
    public class ExtenstionsValueResolver :
        IMemberValueResolver<object, object, ExtensionsCollection, xAPI.ExtensionsDictionary>,
        IMemberValueResolver<object, object, xAPI.ExtensionsDictionary, ExtensionsCollection>
    {
        public ExtensionsCollection Resolve(object source, object destination, xAPI.ExtensionsDictionary sourceMember, ExtensionsCollection destMember, ResolutionContext context)
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

        public xAPI.ExtensionsDictionary Resolve(object source, object destination, ExtensionsCollection sourceMember, xAPI.ExtensionsDictionary destMember, ResolutionContext context)
        {
            var ext = new xAPI.ExtensionsDictionary();
            foreach(var mem in sourceMember)
            {
                ext.Add(mem);
            }
            return ext;
        }
    }
}
