using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Events
{
    public class PoolAddedEvent
    {
        public IPool Pool { get; private set; }

        public PoolAddedEvent(IPool pool)
        {
            Pool = pool;
        }
    }
}