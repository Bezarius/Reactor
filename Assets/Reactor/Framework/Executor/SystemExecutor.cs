using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Components;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Extensions;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems.Executor.Handlers;
using UniRx;
using UnityEngine;

namespace Reactor.Systems.Executor
{

    public sealed class SystemHandlerManager : ISystemHandlerManager
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

    public sealed class CoreManager : ICoreManager
    {
        public IPoolManager PoolManager { get; private set; }
        public ISystemHandlerManager HandlerManager { get; private set; }

        public CoreManager(IPoolManager poolManager, ISystemHandlerManager handlerManager, ISystemExecutor systemExecutor)
        {
            PoolManager = poolManager;
            HandlerManager = handlerManager;
            systemExecutor.Start(this);
        }
    }

    public sealed class SystemExecutor : ISystemExecutor, IDisposable
    {
        private readonly IList<ISystem> _systems;
        private IList<IDisposable> _eventSubscriptions;
        private readonly Dictionary<ISystem, Dictionary<IEntity, SubscriptionToken>> _entitySubscribtionsOnSystems;
        private readonly Dictionary<ISystem, SubscriptionToken> _nonEntitySubscriptions;
        private readonly List<SystemReactor> _systemReactors = new List<SystemReactor>();

        private SystemReactor _emptyReactor;
        private ICoreManager _coreManager;

        public SystemReactor EmptyReactor
        {
            get { return _emptyReactor ?? (_emptyReactor = new SystemReactor(this, new HashSet<Type>())); }
        }

        private IEventSystem EventSystem { get; set; }

        public IPoolManager PoolManager
        {
            get { return _coreManager.PoolManager; }
        }

        public ISystemHandlerManager HandlerManager
        {
            get { return _coreManager.HandlerManager; }
        }

        public IEnumerable<ISystem> Systems { get { return _systems; } }

        public SystemExecutor(IEventSystem eventSystem)
        {
            EventSystem = eventSystem;
            _systems = new List<ISystem>();
            _entitySubscribtionsOnSystems = new Dictionary<ISystem, Dictionary<IEntity, SubscriptionToken>>();
            _nonEntitySubscriptions = new Dictionary<ISystem, SubscriptionToken>();
        }

        public void Start(ICoreManager coreManager)
        {
            _coreManager = coreManager;

            var groupAccessorAddedEventSubscription = EventSystem.Receive<GroupAccessorAddedEvent>().Subscribe(OnGroupAccessorAdded);
            _eventSubscriptions = new List<IDisposable>
            {
                groupAccessorAddedEventSubscription
            };
        }

        public void OnGroupAccessorAdded(GroupAccessorAddedEvent args)
        {
            var hashSet = new HashSet<Type>(args.GroupAccessorToken.ComponentTypes);
            var reactors = _systemReactors.Where(x => x.TargetTypesSet.IsSupersetOf(hashSet));
            foreach (var systemReactor in reactors)
            {
                systemReactor.AddGroupAccessor(args.GroupAccessor);
            }
        }


        public void RemoveSystem(ISystem system)
        {
            _systems.Remove(system);

            var manualSystem = system as IManualSystem;
            if (manualSystem != null)
            {
                HandlerManager.ManualSystemHandler.Stop(manualSystem);
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
                _entitySubscribtionsOnSystems.Add(system, HandlerManager.SetupSystemHandler.Setup((ISetupSystem)system)
                    .ToDictionary(x => x.AssociatedEntity));
            }
            if (system is IGroupReactionSystem)
            {
                _nonEntitySubscriptions.Add(system, HandlerManager.GroupReactionSystemHandler.Setup((IGroupReactionSystem)system));
            }
            if (system is IEntityReactionSystem)
            {
                _entitySubscribtionsOnSystems.Add(system, HandlerManager.EntityReactionSystemHandler.Setup((IEntityReactionSystem)system)
                    .ToDictionary(x => x.AssociatedEntity));
            }
            if (system is IInteractReactionSystem)
            {
                _entitySubscribtionsOnSystems.Add(system, HandlerManager.InteractReactionSystemHandler.Setup((IInteractReactionSystem)system)
                    .ToDictionary(x => x.AssociatedEntity));
            }
            if (system is IManualSystem)
            {
                HandlerManager.ManualSystemHandler.Start((IManualSystem)system);
            }
        }


        public SystemReactor GetOrCreateConcreteSystemReactor(IList<Type> types)
        {
            SystemReactor reactor = null;
            foreach (var systemReactor in _systemReactors)
            {
                if (systemReactor.TargetTypesList.Count == types.Count)
                {
                    for (int i = 0; i < types.Count; i++)
                    {
                        if (systemReactor.TargetTypesList[i] == types[i] && i == types.Count)
                        {
                            reactor = systemReactor;
                        }
                    }
                }
            }
            if (reactor == null)
            {
                reactor = new SystemReactor(this, new HashSet<Type>(types));
                string typeNames = reactor.TargetTypesList.Select(x => x.Name).Aggregate(string.Empty,
                    (current, typeName) => current + string.Format("{0}; ", typeName));
                //Debug.Log(string.Format("created new reactor with types: {0}", typeNames));
                _systemReactors.Add(reactor);
            }
            else
            {
                string typeNames = reactor.TargetTypesList.Select(x => x.Name).Aggregate(string.Empty,
                    (current, typeName) => current + string.Format("{0}; ", typeName));
                //Debug.Log(string.Format("Using existing reactor  with types: {0}", typeNames));
            }
            return reactor;
        }


        /// <summary>
        /// Не учитывает порядок типов в сущности. Использовать нужно осторожно, т.к. может привести к несоответствию индекса компонентов
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public SystemReactor GetSystemReactor(IEnumerable<IComponent> components)
        {
            var hs = new HashSet<Type>();
            foreach (var component in components)
            {
                hs.Add(component.GetType());
            }
            return GetSystemReactor(hs);
        }

        /// <summary>
        /// Не учитывает порядок типов в сущности. Использовать нужно осторожно, т.к. может привести к несоответствию индекса компонентов
        /// </summary>
        /// <param name="targetTypes"></param>
        /// <returns></returns>
        public SystemReactor GetSystemReactor(HashSet<Type> targetTypes)
        {
            if (targetTypes.Count > 0)
            {
                SystemReactor reactor =
                    _systemReactors.FirstOrDefault(
                        x => x.TargetTypesSet.SetEquals(targetTypes));



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
            if (container.HasGroupOrSystems)
            {
                for (int i = 0; i < container.SetupSystems.Length; i++)
                {
                    var system = container.SetupSystems[i];
                    var subscription = HandlerManager.SetupSystemHandler.ProcessEntity(system, entity);
                    if (subscription != null)
                    {
                        _entitySubscribtionsOnSystems[system].Add(entity, subscription);
                    }
                }

                for (int i = 0; i < container.EntityReactionSystems.Length; i++)
                {
                    var system = container.EntityReactionSystems[i];
                    var subscription = HandlerManager.EntityReactionSystemHandler.ProcessEntity(system, entity);
                    if (subscription != null)
                    {
                        _entitySubscribtionsOnSystems[system].Add(entity, subscription);
                    }
                }

                for (int i = 0; i < container.InteractReactionSystems.Length; i++)
                {
                    var system = container.InteractReactionSystems[i];
                    var subscription = HandlerManager.InteractReactionSystemHandler.ProcessEntity(system, entity);
                    if (subscription != null)
                    {
                        _entitySubscribtionsOnSystems[system].Add(entity, subscription);
                    }
                }

                for (int i = 0; i < container.GroupAccessors.Length; i++)
                {
                    var accessor = container.GroupAccessors[i];
                    accessor.AddEntity(entity);
                }
            }
        }

        private void RemoveEntitySubscriptionFromSystem(IEntity entity, ISystem system)
        {
            try
            {
                var subscriptionTokens = _entitySubscribtionsOnSystems[system][entity];
                subscriptionTokens.Disposable.Dispose();
                _entitySubscribtionsOnSystems[system].Remove(entity);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Remove '{0}' subscription form entity with id: '{1}' error:\n{2}", system.GetType().Name, entity.Id, e));
            }
        }

        public void RemoveSystemsFromEntity(IEntity entity, ISystemContainer container)
        {
            if (container.HasGroupOrSystems)
            {
                for (int i = 0; i < container.TeardownSystems.Length; i++)
                {
                    var system = container.TeardownSystems[i];

                    system.Teardown(entity);
                }

                for (int i = 0; i < container.EntityReactionSystems.Length; i++)
                {
                    RemoveEntitySubscriptionFromSystem(entity, container.EntityReactionSystems[i]);
                }

                for (int i = 0; i < container.InteractReactionSystems.Length; i++)
                {
                    RemoveEntitySubscriptionFromSystem(entity, container.InteractReactionSystems[i]);
                }

                for (int i = 0; i < container.GroupAccessors.Length; i++)
                {
                    var accessor = container.GroupAccessors[i];
                    accessor.RemoveEntity(entity);
                }
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