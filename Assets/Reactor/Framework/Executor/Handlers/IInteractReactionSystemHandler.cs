using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Systems.Executor.Handlers
{
    public interface IInteractReactionSystemHandler
    {
        IPoolManager PoolManager { get; }
        IEnumerable<SubscriptionToken> Setup(IInteractReactionSystem system);
        SubscriptionToken ProcessEntity(IInteractReactionSystem system, IEntity entity);
    }
}