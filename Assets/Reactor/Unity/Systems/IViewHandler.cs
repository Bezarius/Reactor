using System;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Pools;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.Systems
{
    public interface IViewHandler
    {
        IPoolManager PoolManager { get; }
        IEventSystem EventSystem { get; }
        IInstantiator Instantiator { get; }

        GameObject InstantiateAndInject(GameObject prefab,
            Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion));

        void DestroyView(GameObject view);
        void SetupView(IEntity entity, Func<IEntity, GameObject> viewResolver);
    }
}