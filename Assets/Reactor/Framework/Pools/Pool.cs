using System.Collections.Generic;
using Reactor.Blueprints;
using Reactor.Entities;
using Reactor.Events;

namespace Reactor.Pools
{
    public class Pool : IPool
    {
        private readonly HashSet<IEntity> _entities;

        public string Name { get; private set; }
        public IEnumerable<IEntity> Entities { get { return _entities;} }
        public IEventSystem EventSystem { get; private set; }
        public IEntityFactory EntityFactory { get; private set; }

        public Pool(string name, IEntityFactory entityFactory, IEventSystem eventSystem)
        {
            _entities = new HashSet<IEntity>();
            Name = name;
            EventSystem = eventSystem;
            EntityFactory = entityFactory;
        }

        public IEntity CreateEntity(IBlueprint blueprint = null)
        {
            var entity = EntityFactory.Create(this, null);

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

            entity.Dispose();

            EventSystem.Publish(new EntityRemovedEvent(entity, this));
        }
    }
}
