using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Systems.Executor.Handlers
{
    public interface ISetupSystemHandler
    {
        IPoolManager PoolManager { get; }
        IEnumerable<SubscriptionToken> Setup(ISetupSystem system);
        SubscriptionToken ProcessEntity(ISetupSystem system, IEntity entity);
    }
}