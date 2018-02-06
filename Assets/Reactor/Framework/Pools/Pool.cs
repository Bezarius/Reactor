using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Components;
using Reactor.Blueprints;
using Reactor.Components;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Groups;
using Reactor.Systems.Executor;
using UnityEngine.Assertions;

namespace Reactor.Pools
{
    public class Pool : IPool
    {
        private readonly IEntityIndexPool _indexPool;
        private readonly ISystemExecutor _executor;
        private readonly HashSet<IEntity> _entities;

        public string Name { get; private set; }
        public IEnumerable<IEntity> Entities { get { return _entities;} }
        public IEventSystem EventSystem { get; private set; }
        public IEntityFactory EntityFactory { get; private set; }

        // todo: move to global index
        private Dictionary<Type, SystemReactor> _reactorIndex = new Dictionary<Type, SystemReactor>();

        public Pool(string name, IEntityFactory entityFactory, IEntityIndexPool indexPool, ISystemExecutor executor, IEventSystem eventSystem)
        {
            _indexPool = indexPool;
            _executor = executor;
            _entities = new HashSet<IEntity>();
            Name = name;
            EventSystem = eventSystem;
            EntityFactory = entityFactory;
        }

        public IEntity BuildEntity<T>(T blueprint) where T : class, IBlueprint
        {
            Assert.IsNotNull(blueprint);

            var type = typeof(T);
            var components = blueprint.Build().ToList();

            SystemReactor reactor;

            if (!_reactorIndex.TryGetValue(type, out reactor))
            {
                var hs = new HashSet<Type>();
                foreach (var component in components)
                {
                    hs.Add(component.GetType());
                }
                // todo: находит реактор, у которого индекс не совпадает с сущностью
                reactor = _executor.GetSystemReactor(hs);  
            }

            Assert.IsNotNull(reactor);

            var sortedComponents = new IComponent[components.Count];
            foreach (var component in components)
            {
                sortedComponents[reactor.GetComponentIdx(component.TypeId)] = component;
            }

            var entity = EntityFactory.Create(this, _indexPool.GetId(), sortedComponents, reactor);

            _entities.Add(entity);

            reactor.AddEntityToReactor(entity);

            EventSystem.Publish(new EntityAddedEvent(entity, this));

            return entity;
        }

        public IEntity CreateEntity()
        {
            var components = new List<IComponent>();

            SystemReactor reactor = _executor.GetSystemReactor(components);

            Assert.IsNotNull(reactor);

            var entity = EntityFactory.Create(this, _indexPool.GetId(), components, reactor);

            _entities.Add(entity);

            reactor.AddEntityToReactor(entity);

            EventSystem.Publish(new EntityAddedEvent(entity, this));

            return entity;
        }

        public IEntity CreateEntity(IEnumerable<IComponent> components)
        {
            SystemReactor reactor = _executor.GetSystemReactor(components);

            Assert.IsNotNull(reactor);

            var sortedComponents = new IComponent[components.Count()];
            foreach (var component in components)
            {
                sortedComponents[reactor.GetComponentIdx(component.TypeId)] = component;
            }

            var entity = EntityFactory.Create(this, _indexPool.GetId(), sortedComponents, reactor);

            _entities.Add(entity);

            reactor.AddEntityToReactor(entity);

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
