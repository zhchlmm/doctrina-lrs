using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Application.Infrastructure.Mapping
{
    public interface IMapperContext
    {
        TTarget MapFrom<TTarget>(object source) where TTarget : new();
        TTarget MapFrom<TTarget, TSource>(TSource source, TTarget target);
    }
}
