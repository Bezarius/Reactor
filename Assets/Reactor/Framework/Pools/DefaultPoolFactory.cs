using Reactor.Entities;
using Reactor.Events;

namespace Reactor.Pools
{
    public class DefaultPoolFactory : IPoolFactory
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IEventSystem _eventSystem;

        public DefaultPoolFactory(IEntityFactory entityFactory, IEventSystem eventSystem)
        {
            _entityFactory = entityFactory;
            _eventSystem = eventSystem;
        }

        public IPool Create(string name)
        {
            return new Pool(name, _entityFactory, _eventSystem);
        }
    }
}