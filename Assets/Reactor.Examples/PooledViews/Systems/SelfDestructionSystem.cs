using System;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems;
using Reactor.Unity.Components;
using UniRx;

namespace Assets.Reactor.Examples.PooledViews.Systems
{
    public class SelfDestructionSystem : IEntityReactionSystem
    {
        public IGroup TargetGroup { get; private set; }

        private readonly IPool _defaultPool;

        public SelfDestructionSystem(IPoolManager poolManager)
        {
            TargetGroup = new Group(typeof(SelfDestructComponent), typeof(ViewComponent));
            _defaultPool = poolManager.GetPool();
        }

        public IObservable<IEntity> Impact(IEntity entity)
        {
            var selfDestructComponent = entity.GetComponent<SelfDestructComponent>();
            return Observable.Interval(TimeSpan.FromSeconds(selfDestructComponent.Lifetime)).Select(x => entity);
        }

        public void Reaction(IEntity entity)
        {
            _defaultPool.RemoveEntity(entity);
        }
    }
}