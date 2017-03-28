using System.Linq;
using Reactor.Groups;
using Reactor.Pools;
using UniRx;

namespace Reactor.Systems.Executor.Handlers
{
    public class GroupReactionSystemHandler : IGroupReactionSystemHandler
    {
        public IPoolManager PoolManager { get; private set; }

        public GroupReactionSystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public SubscriptionToken Setup(IGroupReactionSystem system)
        {
            //var hasEntityPredicate = system.TargetGroup is IHasPredicate;
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            var subscription = system.Impact(groupAccessor)
                .Subscribe(accessor =>
                {
                    foreach (var entity in accessor.Entities)
                    {
                        system.Reaction(entity);
                    }
                    /*
                    var entities = accessor.Entities.ToList();
                    var entityCount = entities.Count - 1;
                    for (var i = entityCount; i >= 0; i--)
                    {
                        if (hasEntityPredicate)
                        {
                            var groupPredicate = system.TargetGroup as IHasPredicate;
                            if (groupPredicate.CanProcessEntity(entities[i]))
                            {
                                system.Reaction(entities[i]);
                            }
                            return;
                        }

                        system.Reaction(entities[i]);
                    }*/
                });

            return new SubscriptionToken(null, subscription);
        }
    }
}
