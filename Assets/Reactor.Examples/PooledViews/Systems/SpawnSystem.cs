using System;
using Assets.Reactor.Examples.PooledViews.ViewResolvers;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using UniRx;
using UnityEngine;

namespace Assets.Reactor.Examples.PooledViews.Systems
{
    public class SpawnSystem : IEntityReactionSystem
    {
        private readonly IDestructableFactory _factory;

        public IGroup TargetGroup { get; private set; }

        public SpawnSystem(IDestructableFactory factory)
        {
            _factory = factory;

            TargetGroup = new GroupBuilder()
                .WithComponent<SpawnerComponent>()
                .WithComponent<ViewComponent>()
                .Build();
        }

        public IObservable<IEntity> Impact(IEntity entity)
        {
            var spawnComponent = entity.GetComponent<SpawnerComponent>();

            return Observable
                .Interval(TimeSpan.FromSeconds(spawnComponent.SpawnRate))
                .Select(x => entity);
        }

        public void Reaction(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var tr = viewComponent.GameObject.transform;
            _factory.Create(DestructableTypes.PooledPrefab, tr.position, tr.rotation, Vector3.one);
        }
    }
}