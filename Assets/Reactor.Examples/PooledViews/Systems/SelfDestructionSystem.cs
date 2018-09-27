using System;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using UniRx;

namespace Assets.Reactor.Examples.PooledViews.Systems
{
    public class SelfDestructionSystem : IEntityReactionSystem
    {
        public IGroup TargetGroup { get; private set; }

        public SelfDestructionSystem()
        {
            TargetGroup = new GroupBuilder()
                .WithComponent<SelfDestructComponent>()
                .WithComponent<ViewComponent>()
                .Build();
        }

        public IObservable<IEntity> Impact(IEntity entity)
        {
            var selfDestructComponent = entity.GetComponent<SelfDestructComponent>();
            return Observable.Interval(TimeSpan.FromSeconds(selfDestructComponent.Lifetime)).Select(x => entity);
        }

        public void Reaction(IEntity entity)
        {
            entity.Destory();
        }
    }
}