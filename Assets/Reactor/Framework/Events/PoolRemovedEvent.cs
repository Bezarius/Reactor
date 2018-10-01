using Reactor.Pools;

namespace Reactor.Events
{
    public class PoolRemovedEvent
    {
        public IPool Pool { get; }

        public PoolRemovedEvent(IPool pool)
        {
            Pool = pool;
        }
    }
}