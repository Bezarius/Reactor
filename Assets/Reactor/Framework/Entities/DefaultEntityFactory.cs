using Reactor.Events;
using Reactor.Pools;

namespace Reactor.Entities
{
    public class DefaultEntityFactory : IEntityFactory
    {
        private readonly IEventSystem _eventSystem;

        public DefaultEntityFactory(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public IEntity Create(IPool pool, int entityId)
        {
            return new Entity(entityId, pool, _eventSystem);
        }
    }
}