using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Extensions;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems.Executor.Handlers;
using UniRx;

namespace Reactor.Systems.Executor
{

    public sealed class SystemHandlerManager
    {
        public IEntityReactionSystemHandler EntityReactionSystemHandler { get; private set; }
        public IGroupReactionSystemHandler GroupReactionSystemHandler { get; private set; }
        public ISetupSystemHandler SetupSystemHandler { get; private set; }
        public IInteractReactionSystemHandler InteractReactionSystemHandler { get; private set; }
        public IManualSystemHandler ManualSystemHandler { get; private set; }


        public SystemHandlerManager(
                IEntityReactionSystemHandler entityReactionSystemHandler,
                IGroupReactionSystemHandler groupReactionSystemHandler,
                ISetupSystemHandler setupSystemHandler,
                IInteractReactionSystemHandler interactReactionSystemHandler,
                IManualSystemHandler manualSystemHandler)
        {
            EntityReactionSystemHandler = entityReactionSystemHandler;
            GroupReactionSystemHandler = groupReactionSystemHandler;
            SetupSystemHandler = setupSystemHandler;
            InteractReactionSystemHandler = interactReactionSystemHandler;
            ManualSystemHandler = manualSystemHandler;
        }
    }

    public sealed class SystemExecutor : ISystemExecutor, IDisposable
    {
        private readonly IList<ISystem> _systems;
        private readonly IList<IDisposable> _eventSubscriptions;
        private readonly Dictionary<ISystem, Dictionary<IEntity, SubscriptionToken>> _entitySubscribtionsOnSystems;
        private readonly Dictionary<ISystem, SubscriptionToken> _nonEntitySubscriptions;
        private readonly List<SystemReactor> _systemReactors = new List<SystemReactor>();

        private SystemReactor _emptyReactor;
        private SystemReactor EmptyReactor
        {
            get { return _emptyReactor ?? (_emptyReactor = new SystemReactor(this, new HashSet<Type>())); }
        }

        public IEventSystem EventSystem { get; private set; }
        public IPoolManager PoolManager { get; private set; }
        public IEnumerable<ISystem> Systems { get { return _systems; } }

        public IEntityReactionSystemHandler EntityReactionSystemHandler { get; private set; }
        public IGroupReactionSystemHandler GroupReactionSystemHandler { get; private set; }
        public ISetupSystemHandler SetupSystemHandler { get; private set; }
        public IInteractReactionSystemHandler InteractReactionSystemHandler { get; private set; }
        public IManualSystemHandler ManualSystemHandler { get; private set; }

        public SystemExecutor(
            IPoolManager poolManager,
            IEventSystem eventSystem,
            IEntityReactionSystemHandler entityReactionSystemHandler,
            IGroupReactionSystemHandler groupReactionSystemHandler,
            ISetupSystemHandler setupSystemHandler,
            IInteractReactionSystemHandler interactReactionSystemHandler,
            IManualSystemHandler manualSystemHandler)
        {
            PoolManager = poolManager;
            EventSystem = eventSystem;
            EntityReactionSystemHandler = entityReactionSystemHandler;
            GroupReactionSystemHandler = groupReactionSystemHandler;
            SetupSystemHandler = setupSystemHandler;
            InteractReactionSystemHandler = interactReactionSystemHandler;
            ManualSystemHandler = manualSystemHandler;

            var addEntitySubscription = EventSystem.Receive<EntityAddedEvent>().Subscribe(OnEntityAddedToPool);
            var removeEntitySubscription = EventSystem.Receive<EntityRemovedEvent>().Subscribe(OnEntityRemovedFromPool);
            var addComponentSubscription = EventSystem.Receive<ComponentAddedEvent>().Subscribe(OnEntityComponentAdded);
            var removeComponentSubscription = EventSystem.Receive<ComponentRemovedEvent>().Subscribe(OnEntityComponentRemoved);

            _systems = new List<ISystem>();
            _entitySubscribtionsOnSystems = new Dictionary<ISystem, Dictionary<IEntity, SubscriptionToken>>();
            _nonEntitySubscriptions = new Dictionary<ISystem, SubscriptionToken>();
            _eventSubscriptions = new List<IDisposable>
            {
                addEntitySubscription,
                removeEntitySubscription,
                addComponentSubscription,
                removeComponentSubscription
            };
        }


        public void OnEntityComponentAdded(ComponentAddedEvent args)
        {
            var entity = args.Entity;
            var type = args.Component.GetType();
            if (entity.Reactor != null)
            {
                args.Entity.Reactor.AddComponent(entity, type);
            }
            else
            {
                var reactor = this.GetSystemReactor(new HashSet<Type> { type });
                entity.Reactor = reactor;
                AddSystemsToEntity(entity, reactor);
            }
        }

        public void OnEntityComponentRemoved(ComponentRemovedEvent args)
        {
            args.Entity.Reactor.RemoveComponent(args.Entity, args.Component);
        }

        public void OnEntityAddedToPool(EntityAddedEvent args)
        {
        }

        public void OnEntityRemovedFromPool(EntityRemovedEvent args)
        {
        }


        public void RemoveSystem(ISystem system)
        {
            _systems.Remove(system);

            var manualSystem = system as IManualSystem;
            if (manualSystem != null)
            {
                ManualSystemHandler.Stop(manualSystem);
            }

            if (_entitySubscribtionsOnSystems.ContainsKey(system))
            {
                _entitySubscribtionsOnSystems[system].Values.DisposeAll();
                _entitySubscribtionsOnSystems.Remove(system);
            }
            else if (_nonEntitySubscriptions.ContainsKey(system))
            {
                _nonEntitySubscriptions[system].Disposable.Dispose();
                _nonEntitySubscriptions.Remove(system);
            }
        }

        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
            if (system is ISetupSystem)
            {
                _entitySubscribtionsOnSystems.Add(system, SetupSystemHandler.Setup((ISetupSystem)system)
                    .ToDictionary(x => x.AssociatedObject as IEntity));
            }
            else if (system is IGroupReactionSystem)
            {
                _nonEntitySubscriptions.Add(system, GroupReactionSystemHandler.Setup((IGroupReactionSystem)system));
            }
            else if (system is IEntityReactionSystem)
            {
                _entitySubscribtionsOnSystems.Add(system, EntityReactionSystemHandler.Setup((IEntityReactionSystem)system)
                    .ToDictionary(x => x.AssociatedObject as IEntity));
            }
            else if (system is IInteractReactionSystem)
            {
                _entitySubscribtionsOnSystems.Add(system, InteractReactionSystemHandler.Setup((IInteractReactionSystem)system)
                    .ToDictionary(x => x.AssociatedObject as IEntity));
            }
            else if (system is IManualSystem)
            {
                ManualSystemHandler.Start((IManualSystem)system);
            }
        }

        
        //todo: добавить оптимизацию для сущностей которые создаются с помощью блюпринтов. Даст прирост ~5%
        public SystemReactor GetSystemReactor(HashSet<Type> targetTypes)
        {
            if (targetTypes.Count > 0)
            {
                SystemReactor reactor =
                    _systemReactors.FirstOrDefault(
                        x => x.TargetTypes.SetEquals(targetTypes));

                if (reactor == null)
                {
                    reactor = new SystemReactor(this, targetTypes);
                    _systemReactors.Add(reactor);
                }

                return reactor;
            }
            return EmptyReactor;
        }

        public void AddSystemsToEntity(IEntity entity, ISystemContainer container)
        {
            for (int i = 0; i < container.SetupSystems.Length; i++)
            {
                var system = container.SetupSystems[i];
                var subscription = SetupSystemHandler.ProcessEntity(system, entity);
                if (subscription != null)
                {
                    _entitySubscribtionsOnSystems[system].Add(entity, subscription);
                }
            }

            for (int i = 0; i < container.EntityReactionSystems.Length; i++)
            {
                var system = container.EntityReactionSystems[i];
                var subscription = EntityReactionSystemHandler.ProcessEntity(system, entity);
                if (subscription != null)
                {
                    _entitySubscribtionsOnSystems[system].Add(entity, subscription);
                }
            }

            for (int i = 0; i < container.InteractReactionSystems.Length; i++)
            {
                var system = container.InteractReactionSystems[i];
                var subscription = InteractReactionSystemHandler.ProcessEntity(system, entity);
                if (subscription != null)
                {
                    _entitySubscribtionsOnSystems[system].Add(entity, subscription);
                }
            }
        }

        private void RemoveEntitySubscriptionFromSystem(IEntity entity, ISystem system)
        {
            var subscriptionTokens = _entitySubscribtionsOnSystems[system][entity];
            subscriptionTokens.Disposable.Dispose();
            _entitySubscribtionsOnSystems[system].Remove(entity);
        }

        public void RemoveSystemsFromEntity(IEntity entity, ISystemContainer container)
        {
            for (int i = 0; i < container.TeardownSystems.Length; i++)
            {
                var system = container.TeardownSystems[i];

                system.Teardown(entity);

                RemoveEntitySubscriptionFromSystem(entity, system);
            }

            for (int i = 0; i < container.EntityReactionSystems.Length; i++)
            {
                RemoveEntitySubscriptionFromSystem(entity, container.EntityReactionSystems[i]);
            }

            for (int i = 0; i < container.InteractReactionSystems.Length; i++)
            {
                RemoveEntitySubscriptionFromSystem(entity, container.InteractReactionSystems[i]);
            }
        }

        public int GetSubscriptionCountForSystem(ISystem system)
        {
            if (!_entitySubscribtionsOnSystems.ContainsKey(system))
            {
                return 0;
            }
            return _entitySubscribtionsOnSystems[system].Count;
        }

        public int GetTotalSubscriptions()
        {
            return _entitySubscribtionsOnSystems.Values.Sum(x => x.Count);
        }

        public void Dispose()
        {
            _entitySubscribtionsOnSystems.ForEachRun(x => x.Value.Values.DisposeAll());
            _eventSubscriptions.DisposeAll();
        }
    }
}