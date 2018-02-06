using Reactor.Entities;
using Reactor.Events;
using Reactor.Systems.Executor;

namespace Reactor.Pools
{
    public class DefaultPoolFactory : IPoolFactory
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IEventSystem _eventSystem;
        private readonly IEntityIndexPool _entityIndexPool;
        private readonly ISystemExecutor _systemExecutor;

        public DefaultPoolFactory(IEntityFactory entityFactory, IEventSystem eventSystem, IEntityIndexPool entityIndexPool, ISystemExecutor systemExecutor)
        {
            _entityFactory = entityFactory;
            _eventSystem = eventSystem;
            _entityIndexPool = entityIndexPool;
            _systemExecutor = systemExecutor;
        }

        public IPool Create(string name)
        {
            return new Pool(name, _entityFactory, _entityIndexPool, _systemExecutor, _eventSystem);
        }
    }
}