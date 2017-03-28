using Assets.Reactor.Examples.CustomGameObjectHandling.Components;
using Reactor.Entities;
using Reactor.Extensions;
using Reactor.Groups;
using Reactor.Systems;
using UnityEngine;

namespace Assets.Reactor.Examples.CustomGameObjectHandling.Systems
{
    public class CustomViewSetupSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new Group().WithComponent<CustomViewComponent>();} }
        
        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<CustomViewComponent>();
            viewComponent.CustomView = GameObject.CreatePrimitive(PrimitiveType.Cube);
            viewComponent.CustomView.name = "entity-" + entity.Id;
            var rigidBody = viewComponent.CustomView.AddComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
        }
    }
}