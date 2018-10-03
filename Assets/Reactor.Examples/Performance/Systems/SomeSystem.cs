using System;
using Assets.Reactor.Examples.Performance.Components;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using UniRx;

namespace Assets.Reactor.Examples.Performance.Systems
{
    public class SomeSystem : IEntityReactionSystem
    {
        public IGroup TargetGroup { get; } = 
            new GroupBuilder()
                .WithComponent<Component1>()
                .WithComponent<Component2>()
                .WithComponent<Component3>()
                .Build();

        public IObservable<IEntity> Impact(IEntity entity)
        {
            return Observable.EveryUpdate().Select(x => entity);
        }

        public void Reaction(IEntity entity)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                entity.RemoveComponent<Component2>();
                entity.AddComponent(new Component4());
            }
            else
            {
                entity.RemoveComponent<Component3>();
                entity.AddComponent(new Component5());
            }
        }
    }

    public class SomeSystem2 : IEntityReactionSystem
    {
        public IGroup TargetGroup { get; } =
            new GroupBuilder()
                .WithComponent<Component1>()
                .WithComponent<Component3>()
                .WithComponent<Component4>()
                .Build();


        public IObservable<IEntity> Impact(IEntity entity)
        {
            return Observable.EveryUpdate().Select(x => entity);
        }

        public void Reaction(IEntity entity)
        {
            entity.RemoveComponent<Component4>();
            entity.AddComponent(new Component2());
        }
    }

    public class SomeSystem3 : IEntityReactionSystem
    {
        public IGroup TargetGroup { get; } =
            new GroupBuilder()
                .WithComponent<Component1>()
                .WithComponent<Component2>()
                .WithComponent<Component5>()
                .Build();


        public IObservable<IEntity> Impact(IEntity entity)
        {
            return Observable.EveryUpdate().Select(x => entity);
        }

        public void Reaction(IEntity entity)
        {
            entity.RemoveComponent<Component5>();
            entity.AddComponent(new Component3());
        }
    }
}