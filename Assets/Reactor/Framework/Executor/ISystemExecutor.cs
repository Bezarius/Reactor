using System;
using System.Collections.Generic;
using Reactor.Components;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Pools;

namespace Reactor.Systems.Executor
{
    public interface ISystemExecutor
    {
        IPoolManager PoolManager { get; }
        IEnumerable<ISystem> Systems { get; }

        void Start(ICoreManager coreManager);

        void RemoveSystem(ISystem system);
        void AddSystem(ISystem system);
        SystemReactor GetOrCreateConcreteSystemReactor(IList<Type> types);
        SystemReactor GetSystemReactor(IEnumerable<IComponent> targetTypes);
        SystemReactor GetSystemReactor(HashSet<Type> targetTypes);
        void AddSystemsToEntity(IEntity entity, ISystemContainer container);
        void RemoveSystemsFromEntity(IEntity entity, ISystemContainer connection);
    }

    public interface ICoreManager
    {
        IPoolManager PoolManager { get; }
        ISystemHandlerManager HandlerManager { get; }
    }
}