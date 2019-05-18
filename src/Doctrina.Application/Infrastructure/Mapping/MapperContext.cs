using Doctrina.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Application.Infrastructure.Mapping
{
    public sealed class MapperContext : IMapperContext
    {
        private readonly IList<Type> _mapperActions;

        public MapperContext()
        {
            _mapperActions = LoadMappings(GetType().Assembly).ToList();
        }

        public TTarget MapFrom<TTarget, TSource>(TSource source) where TTarget : new()
        {
            return InvokeMethod(source, new TTarget());
        }

        public TTarget MapFrom<TTarget>(object source) where TTarget : new()
        {
            return InvokeMethod(source, new TTarget());
        }

        public TTarget MapFrom<TTarget, TSource>(TSource source, TTarget target)
        {
            return InvokeMethod(source, target);
        }

        private TTarget InvokeMethod<TTarget, TSource>(TSource source, TTarget target)
        {
            Type sourceType = source.GetType();
            Type targetType = target.GetType();

            Type mapperAction = FindAction(sourceType, targetType);

            var instance = (IMapFrom<TSource, TTarget>)Activator.CreateInstance(mapperAction);

            return instance.MapFrom(source, target);
        }

        private Type FindAction(Type sourceType, Type targetType)
        {
            var mapperAction = (
                from action in _mapperActions
                let method = action.GetMethod("MapFrom", new Type[] { sourceType, targetType })
                where method != null
                select action).FirstOrDefault();

            if (mapperAction == null)
            {
                throw new NullReferenceException($"Unable to find IMapperAction for source '{sourceType.Name}' and target {targetType.Name}");
            }

            return mapperAction;
        }

        private IEnumerable<Type> LoadMappings(Assembly rootAssembly)
        {
            var types = rootAssembly.GetExportedTypes();

            var mapsFrom = (
                    from type in types
                    from instance in type.GetInterfaces()
                    where
                        instance.IsGenericType && instance.GetGenericTypeDefinition() == typeof(IMapFrom<,>) &&
                        !type.IsAbstract &&
                        !type.IsInterface
                    select type);

            return mapsFrom;
        }

       
    }
}
