using Assets.Reactor.Unity.ViewPooling;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Pools;
using Reactor.Unity.MonoBehaviours;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.Systems
{
    public abstract class DefaultInjectablePooledViewResolverSystem : PooledInjectableViewResolverSystem
    {
        public IViewPool ViewPool { get; private set; }

        protected DefaultInjectablePooledViewResolverSystem(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator) :
            base(poolManager, eventSystem, instantiator)
        {
            ViewPool = new InjectableViewPool(instantiator, PrefabTemplate);
        }

        protected override void RecycleView(GameObject viewToRecycle)
        {
            viewToRecycle.transform.parent = null;
            var entityView = viewToRecycle.GetComponent<EntityView>();
            entityView.Entity = null;
            ViewPool.ReleaseInstance(viewToRecycle);
        }

        protected override GameObject AllocateView(IEntity entity)
        {
            var viewToAllocate = ViewPool.AllocateInstance();
            var entityView = viewToAllocate.GetComponent<EntityView>();
            entityView.Entity = entity;
            return viewToAllocate;
        }
    }
}