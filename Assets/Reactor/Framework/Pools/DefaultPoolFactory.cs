using Reactor.Entities;
using Reactor.Events;

namespace Reactor.Pools
{
    public class DefaultPoolFactory : IPoolFactory
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IEventSystem _eventSystem;
        private readonly IEntityIndexPool _entityIndexPool;

        public DefaultPoolFactory(IEntityFactory entityFactory, IEventSystem eventSystem, IEntityIndexPool entityIndexPool)
        {
            _entityFactory = entityFactory;
            _eventSystem = eventSystem;
            _entityIndexPool = entityIndexPool;
        }

        public IPool Create(string name)
        {
            return new Pool(name, _entityFactory, _entityIndexPool, _eventSystem);
        }
    }
}