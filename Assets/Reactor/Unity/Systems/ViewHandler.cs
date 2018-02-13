using System;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Pools;
using Reactor.Unity.Components;
using Reactor.Unity.MonoBehaviours;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Reactor.Unity.Systems
{
    public class ViewHandler : IViewHandler
    {
        public IPoolManager PoolManager { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IInstantiator Instantiator { get; private set; }

        protected ViewHandler(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator)
        {
            PoolManager = poolManager;
            EventSystem = eventSystem;
            Instantiator = instantiator;
        }

        public virtual GameObject InstantiateAndInject(GameObject prefab,
            Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion))
        {
            var createdPrefab = Instantiator.InstantiatePrefab(prefab);
            createdPrefab.transform.position = position;
            createdPrefab.transform.rotation = rotation;
            return createdPrefab;
        }

        public virtual void DestroyView(GameObject view)
        {
            Object.Destroy(view);
        }

        public virtual void SetupView(IEntity entity, Func<IEntity, GameObject> viewResolver)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();

            // check if ViewCmponent.GameObject is already initialized by ViewComponentWrapper
            if (viewComponent.GameObject == null)
            {
                var viewObject = viewResolver(entity);
                viewComponent.GameObject = viewObject;
            }

            var entitiView = viewComponent.GameObject.GetComponent<EntityView>();
            if (entitiView == null)
            {
                entitiView = viewComponent.GameObject.AddComponent<EntityView>();
                entitiView.Entity = entity;
            }
        }
    }
}