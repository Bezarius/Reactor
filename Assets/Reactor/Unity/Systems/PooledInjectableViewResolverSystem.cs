using Reactor.Attributes;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems;
using Reactor.Unity.Components;
using UniRx;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.Systems
{
    
    [Priority(1000)]
    public abstract class PooledInjectableViewResolverSystem : ISetupSystem
    {
        public IPoolManager PoolManager { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IInstantiator Instantiator { get; private set; }

        protected GameObject PrefabTemplate { get; set; }

        public virtual IGroup TargetGroup
        {
            get { return new Group(typeof(ViewComponent)); }
        }
        
        protected PooledInjectableViewResolverSystem(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator)
        {
            PoolManager = poolManager;
            Instantiator = instantiator;
            EventSystem = eventSystem;

            PrefabTemplate = ResolvePrefabTemplate();
        }
        
        protected abstract GameObject ResolvePrefabTemplate();
        protected abstract void RecycleView(GameObject viewToRecycle);
        protected abstract GameObject AllocateView(IEntity entity);

        public virtual void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            if (viewComponent.View != null) { return; }

            var viewObject = AllocateView(entity);
            viewComponent.View = viewObject;

            EventSystem.Receive<EntityRemovedEvent>()
                .First(x => x.Entity == entity)
                .Subscribe(x => RecycleView(viewObject));
        }
    }
}