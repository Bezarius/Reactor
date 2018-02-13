using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Extensions;
using Reactor.Pools;
using UniRx;

namespace Reactor.Systems.Executor.Handlers
{
    public class EntityReactionSystemHandler : IEntityReactionSystemHandler
    {
        public IPoolManager PoolManager { get; private set; }

        public EntityReactionSystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public IEnumerable<SubscriptionToken> Setup(IEntityReactionSystem system)
        {
            var entities = PoolManager.GetEntitiesFor(system.TargetGroup);
            return entities.ForEachRun(x => ProcessEntity(system, x));
        }

        public SubscriptionToken ProcessEntity(IEntityReactionSystem system, IEntity entity)
        {
            //var predicate = system.TargetGroup as IHasPredicate;
            var subscription = system.Impact(entity)
                .Subscribe(x =>
                {
                    system.Reaction(x);
                });

            return new SubscriptionToken(entity, subscription);
        }
    }
}