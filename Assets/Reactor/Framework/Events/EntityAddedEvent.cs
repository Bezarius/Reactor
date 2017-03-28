using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Events
{
    public class EntityAddedEvent
    {
        public IEntity Entity { get; private set; }
        public IPool Pool { get; private set; }

        public EntityAddedEvent(IEntity entity, IPool pool)
        {
            Entity = entity;
            Pool = pool;
        }
    }
}