using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Extensions;
using Reactor.Pools;
using UniRx;

namespace Reactor.Systems.Executor.Handlers
{
    public class InteractReactionSystemHandler : IInteractReactionSystemHandler
    {
        public IPoolManager PoolManager { get; }

        public InteractReactionSystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public IEnumerable<SubscriptionToken> Setup(IInteractReactionSystem system)
        {
            var entities = PoolManager.GetEntitiesFor(system.TargetGroup);
            return entities.ForEachRun(x => ProcessEntity(system, x));
        }

        public SubscriptionToken ProcessEntity(IInteractReactionSystem system, IEntity entity)
        {
            var subscription = system.Impact(entity)
                .Subscribe(x =>
                {
                    system.Reaction(entity, x);
                });

            return new SubscriptionToken(entity, subscription);
        }
    }
}