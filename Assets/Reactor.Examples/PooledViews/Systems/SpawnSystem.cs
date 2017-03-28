using System;
using Assets.Reactor.Examples.PooledViews.Blueprints;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems;
using Reactor.Unity.Components;
using UniRx;

namespace Assets.Reactor.Examples.PooledViews.Systems
{
    public class SpawnSystem : IEntityReactionSystem
    {
        private readonly IPool _defaultPool;

        public IGroup TargetGroup { get; private set; }

        public SpawnSystem(IPoolManager poolManager)
        {
            TargetGroup = new Group(typeof(SpawnerComponent), typeof(ViewComponent));
            _defaultPool = poolManager.GetPool();
        }

        public IObservable<IEntity> Impact(IEntity entity)
        {
            var spawnComponent = entity.GetComponent<SpawnerComponent>();
            return Observable.Interval(TimeSpan.FromSeconds(spawnComponent.SpawnRate)).Select(x => entity);
        }

        public void Reaction(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var blueprint = new SelfDestructBlueprint(viewComponent.View.transform.position);
            _defaultPool.CreateEntity(blueprint); //todo: optimize 68.5%
        }
    }
}