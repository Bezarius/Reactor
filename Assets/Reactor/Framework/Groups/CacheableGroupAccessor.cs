using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Groups
{
    public class CacheableGroupAccessor : IGroupAccessor
    {
        public readonly HashSet<IEntity> CachedEntities;

        public GroupAccessorToken AccessorToken { get; private set; }
        public IEnumerable<IEntity> Entities { get { return CachedEntities; } }

        public CacheableGroupAccessor(GroupAccessorToken accessorToken, IEnumerable<IEntity> initialEntities)
        {
            AccessorToken = accessorToken;
            CachedEntities = new HashSet<IEntity>(initialEntities);
        }

        public void RemoveEntity(IEntity entity)
        {
            CachedEntities.Remove(entity);
        }

        public void AddEntity(IEntity entity)
        {
            CachedEntities.Add(entity);
        }
    }
}