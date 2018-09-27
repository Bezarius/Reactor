﻿using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using UniRx;
using UnityEngine;

namespace Assets.Reactor.Examples.PooledViews.Systems
{
    public class GroupReaction : IGroupReactionSystem
    {
        public IGroup TargetGroup { get; private set; }

        public GroupReaction()
        {
            TargetGroup = new Group(typeof(SpawnerComponent), typeof(ViewComponent));
        }

        public IObservable<IGroupAccessor> Impact(IGroupAccessor @group)
        {
            return Observable.EveryUpdate().Select(x => group);
        }

        public void Reaction(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var pos = viewComponent.GameObject.transform.position;
            viewComponent.GameObject.transform.position = new Vector3(pos.x + UnityEngine.Random.Range(-1, 1f), pos.y + UnityEngine.Random.Range(-1, 1f), pos.z + UnityEngine.Random.Range(-1, 1f));
        }
    }
}