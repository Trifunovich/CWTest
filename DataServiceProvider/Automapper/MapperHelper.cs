using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace DataServiceProvider.Core.Automapper
{
    internal static class MapperHelper
    {
        public static Q MapOneElement<Q, R>(this IMapper mapper, R model)
        {
            return mapper.Map<Q>(model);
        }

        public static IEnumerable<Q> MapList<Q, R>(this IMapper mapper,IEnumerable<R> sourceList)
        {
            return sourceList.Select(mapper.MapOneElement<Q, R>);
        }
    }
}