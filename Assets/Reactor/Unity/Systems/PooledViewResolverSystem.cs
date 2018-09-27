using System.Collections.Generic;
using Assets.Game.SceneCollections;
using Assets.Reactor.Unity.ViewPooling;
using Reactor.Attributes;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using Reactor.Unity.MonoBehaviours;
using UnityEngine;

namespace Reactor.Unity.Systems
{
    [Priority(999)]
    public abstract class PooledViewResolverSystem : ISetupSystem
    {
        public virtual IGroup TargetGroup { get; protected set; }

        private readonly List<IViewPool> _viewPools = new List<IViewPool>();

        protected PooledViewResolverSystem(IPrefabLoader prefabLoader)
        {
            TargetGroup = new GroupBuilder()
                .WithComponent<ViewComponent>()
                .Build();

            // init view pools
            foreach (var prefab in prefabLoader.Prefabs)
            {
                _viewPools.Add(new ViewPool(prefab));
            }
        }

        protected abstract void Resolve(IEntity entity);

        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            if (viewComponent.GameObject == null)
            {
                Resolve(entity);
            }
        }

        protected GameObject AllocateView(IEntity entity, int typeId)
        {
            var pool = _viewPools[typeId];
            var view = entity.GetComponent<ViewComponent>();
            view.ViewPool = pool;
            var allocatedView = pool.AllocateInstance();
            view.GameObject = allocatedView;
            var entityView = allocatedView.GetComponent<EntityView>();
            entityView.Entity = entity;
            return allocatedView;
        }
    }
}