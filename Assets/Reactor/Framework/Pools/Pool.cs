using System.Collections.Generic;
using Reactor.Blueprints;
using Reactor.Entities;
using Reactor.Events;

namespace Reactor.Pools
{
    public class Pool : IPool
    {
        private readonly IEntityIndexPool _indexPool;
        private readonly HashSet<IEntity> _entities;

        public string Name { get; private set; }
        public IEnumerable<IEntity> Entities { get { return _entities;} }
        public IEventSystem EventSystem { get; private set; }
        public IEntityFactory EntityFactory { get; private set; }

        public Pool(string name, IEntityFactory entityFactory, IEntityIndexPool indexPool, IEventSystem eventSystem)
        {
            _indexPool = indexPool;
            _entities = new HashSet<IEntity>();
            Name = name;
            EventSystem = eventSystem;
            EntityFactory = entityFactory;
        }

        public IEntity CreateEntity(IBlueprint blueprint = null)
        {
            var entity = EntityFactory.Create(this, _indexPool.GetId());

            _entities.Add(entity);

            if (blueprint != null)
            {
                blueprint.Apply(entity);
            }

            EventSystem.Publish(new EntityAddedEvent(entity, this));

            return entity;
        }

        public void RemoveEntity(IEntity entity)
        {
            _entities.Remove(entity);
            _indexPool.Release(entity.Id);
            entity.Dispose();

            EventSystem.Publish(new EntityRemovedEvent(entity, this));
        }
    }
}
