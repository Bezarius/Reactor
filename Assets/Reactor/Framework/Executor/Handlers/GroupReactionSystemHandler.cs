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
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            var subscription = system.Impact(groupAccessor)
                .Subscribe(accessor =>
                {
                    foreach (var entity in accessor.Entities)
                    {
                        system.Reaction(entity);
                    }
                });

            return new SubscriptionToken(null, subscription);
        }
    }
}
