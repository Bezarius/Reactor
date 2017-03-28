using Assets.Reactor.Examples.RandomReactions.Components;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Reactor.Examples.RandomReactions.Systems
{
    public class ColorChangingSystem : IGroupReactionSystem, ISetupSystem
    {
        private readonly float MaxDelay = 5.0f;
        private readonly float MinDelay = 1.0f;

        public IGroup TargetGroup
        {
            get { return new Group(typeof (RandomColorComponent)); }
        }

        public void Setup(IEntity entity)
        {
            var randomColorComponent = entity.GetComponent<RandomColorComponent>();
            randomColorComponent.NextChangeIn = Random.Range(MinDelay, MaxDelay);
        }

        public IObservable<IGroupAccessor> Impact(IGroupAccessor @group)
        {
            return Observable.EveryUpdate().Select(x => @group);
        }

        public void Reaction(IEntity entity)
        {
            var randomColorComponent = entity.GetComponent<RandomColorComponent>();
            randomColorComponent.Elapsed += Time.deltaTime;

            if (!(randomColorComponent.Elapsed >= randomColorComponent.NextChangeIn))
            {
                return;
            }

            randomColorComponent.Elapsed -= randomColorComponent.NextChangeIn;
            randomColorComponent.NextChangeIn = Random.Range(MinDelay, MaxDelay);

            var r = Random.Range(0.0f, 1.0f);
            var g = Random.Range(0.0f, 1.0f);
            var b = Random.Range(0.0f, 1.0f);
            randomColorComponent.Color.Value = new Color(r,g,b);
        }
    }
}