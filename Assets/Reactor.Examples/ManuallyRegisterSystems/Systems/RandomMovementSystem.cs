using System;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Reactor.Examples.ManuallyRegisterSystems.Systems
{
    public class RandomMovementSystem : IGroupReactionSystem
    {
        public IGroup TargetGroup { get { return new Group(typeof (ViewComponent)); } }

        public IObservable<IGroupAccessor> Impact(IGroupAccessor @group)
        {
            return Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => @group);
        }

        public void Reaction(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var positionChange = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            viewComponent.GameObject.transform.position += positionChange;
        }
    }
}