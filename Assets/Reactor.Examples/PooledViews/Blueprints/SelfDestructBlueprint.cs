using Reactor.Blueprints;
using Reactor.Entities;
using Reactor.Unity.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.PooledViews.Blueprints
{
    public class SelfDestructBlueprint : IBlueprint
    {
        private readonly float _minLifetime = 2.0f;
        private readonly float _maxLifetime = 5.0f;
        private readonly Vector3 _startPosition;

        public SelfDestructBlueprint(Vector3 startPosition) 
        {
            _startPosition = startPosition;
        }

        public void Apply(IEntity entity)
        {
            /*
            var selfDestructComponent = new SelfDestructComponent
            {
                Lifetime = Random.Range(_minLifetime, _maxLifetime),
                StartingPosition = _startPosition
            };
            var componetns = new List<IComponent>
            {
                selfDestructComponent,
                new ColliderComponent(),
                new ViewComponent()
            };
            entity.AddComponents(componetns);*/

            entity.AddComponent(new SelfDestructComponent
            {
                Lifetime = Random.Range(_minLifetime, _maxLifetime),
                StartingPosition = _startPosition
            });

            entity.AddComponent<ColliderComponent>();
            entity.AddComponent<ViewComponent>();
        }
    }
}