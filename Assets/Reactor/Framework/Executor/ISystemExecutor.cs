using System;
using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Pools;

namespace Reactor.Systems.Executor
{
    public interface ISystemExecutor
    {
        IPoolManager PoolManager { get; }
        IEnumerable<ISystem> Systems { get; }

        void RemoveSystem(ISystem system);
        void AddSystem(ISystem system);
        SystemReactor GetSystemReactor(HashSet<Type> targetTypes);
        void AddSystemsToEntity(IEntity entity, ISystemContainer container);
        void RemoveSystemsFromEntity(IEntity entity, ISystemContainer connection);
    }
}