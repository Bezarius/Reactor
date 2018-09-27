using System.Collections.Generic;
using Assets.Reactor.Examples.PooledViews.ViewResolvers;
using Reactor.Blueprints;
using Reactor.Components;
using Reactor.Unity.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.PooledViews.Blueprints
{
    public class SelfDestructBlueprint : IBlueprint
    {
        private readonly float _minLifetime = 2.0f;
        private readonly float _maxLifetime = 5.0f;

        public IEnumerable<IComponent> Build()
        {
            return new IComponent[]
            {
                new SelfDestructComponent
                {
                    DestructableTypes = DestructableTypes.PooledPrefab,
                    Lifetime = Random.Range(_minLifetime, _maxLifetime)
                }, 
                new ColliderComponent(),
                new ViewComponent()
            };
        }
    }
}