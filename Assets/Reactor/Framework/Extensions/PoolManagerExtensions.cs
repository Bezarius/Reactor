using System.Collections.Generic;
using System.Linq;
using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Extensions
{
    public static class PoolManagerExtensions
    {
        public static IEnumerable<IEntity> GetAllEntities(this IEnumerable<IPool> pools)
        {
            return pools.SelectMany(x => x.Entities);
        }
    }
}