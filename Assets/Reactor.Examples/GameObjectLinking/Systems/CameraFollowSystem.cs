using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.GameObjectLinking.Systems
{
    public class ChangeScaleOnLinkingSystem : ISetupSystem
    {
        private readonly IGroup _targetSystem = new Group(typeof(ViewComponent));

        public IGroup TargetGroup { get { return _targetSystem; } }

        public void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            viewComponent.View.transform.localScale = Vector3.one*3;
        }
    }
}