using System;
using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Pools;
using Reactor.Unity.Components;
using Reactor.Unity.MonoBehaviours;
using UniRx;
using UniRx.Triggers;
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

        private readonly Dictionary<IEntity, IDisposable> _subscriptionsDictionary = new Dictionary<IEntity, IDisposable>();

        protected ViewHandler(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator)
        {
            PoolManager = poolManager;
            EventSystem = eventSystem;
            Instantiator = instantiator;

            // todo: add clear
            EventSystem.Receive<ComponentRemovedEvent>()
                .Where(x => x.Component is ViewComponent)
                .Subscribe(x =>
                {
                    IDisposable viewSubscription;
                    if (_subscriptionsDictionary.TryGetValue(x.Entity, out viewSubscription))
                    {
                        if (viewSubscription != null)
                        {
                            viewSubscription.Dispose();
                        }
                        var view = (x.Component as ViewComponent).View;
                        if(view != null)
                            DestroyView(view);
                    }
                });
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
            if (viewComponent.View != null) { return; }

            var viewObject = viewResolver(entity);
            viewComponent.View = viewObject;

            var entityBinding = viewObject.GetComponent<EntityView>();
            if (entityBinding == null)
            {
                entityBinding = viewObject.AddComponent<EntityView>();
                entityBinding.Entity = entity;
            }

            if (viewComponent.DestroyWithView)
            {
                var viewSubscription = viewObject.OnDestroyAsObservable()
                    .Subscribe(x => entityBinding.Entity.Pool.RemoveEntity(entity))
                    .AddTo(viewObject);

                _subscriptionsDictionary.Add(entity, viewSubscription);
            }
        }
    }
}