using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Systems.Executor.Handlers
{
    public interface IEntityReactionSystemHandler
    {
        IPoolManager PoolManager { get; }
        IEnumerable<SubscriptionToken> Setup(IEntityReactionSystem system);
        SubscriptionToken ProcessEntity(IEntityReactionSystem system, IEntity entity);
    }
}