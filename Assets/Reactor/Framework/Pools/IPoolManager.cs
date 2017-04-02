using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Groups;

namespace Reactor.Pools
{
    public interface IPoolManager
    {
        IEnumerable<IPool> Pools { get; }

        IEnumerable<IEntity> GetEntitiesFor(IGroup group, string poolName = null);
        IGroupAccessor CreateGroupAccessor(IGroup group, string poolName = null);
        IEnumerable<IGroupAccessor> PoolAccessors { get; }

        IPool CreatePool(string name);
        IPool GetPool(string name = null);
        void RemovePool(string name);
    }
}