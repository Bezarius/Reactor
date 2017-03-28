using Reactor.Pools;

namespace Reactor.Systems.Executor.Handlers
{
    public interface IGroupReactionSystemHandler
    {
        IPoolManager PoolManager { get; }
        SubscriptionToken Setup(IGroupReactionSystem system);
    }
}