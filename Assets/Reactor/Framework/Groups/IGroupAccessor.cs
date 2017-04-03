using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Groups
{
    public interface IGroupAccessor
    {
        GroupAccessorToken AccessorToken { get; }
        IEnumerable<IEntity> Entities { get; }
        void RemoveEntity(IEntity entity);
        void AddEntity(IEntity entity);
    }
}