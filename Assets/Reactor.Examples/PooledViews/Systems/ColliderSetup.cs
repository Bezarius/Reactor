﻿using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.PooledViews.Systems
{
    public class ColliderSetup : ISetupSystem
    {
        public IGroup TargetGroup { get; private set; }

        public ColliderSetup()
        {
            TargetGroup = new GroupBuilder()
                .WithComponent<ColliderComponent>()
                .WithComponent<ViewComponent>()
                .Build();
        }

        public void Setup(IEntity entity)
        {
            var view = entity.GetComponent<ViewComponent>();
            var collider = entity.GetComponent<ColliderComponent>();
            collider.Collider = view.GameObject.GetComponent<Collider>();
            collider.Rigidbody = view.GameObject.GetComponent<Rigidbody>();
        }
    }
}