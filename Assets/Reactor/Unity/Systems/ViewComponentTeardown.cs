using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using Reactor.Unity.MonoBehaviours;
using UnityEngine;

namespace Reactor.Unity.Systems
{
    public class ViewComponentTeardown : ITeardownSystem
    {

        public IGroup TargetGroup { get; private set; }

        public ViewComponentTeardown()
        {
            TargetGroup = new GroupBuilder()
                .WithComponent<ViewComponent>()
                .Build();
        }

        public void Teardown(IEntity entity)
        {
            var view = entity.GetComponent<ViewComponent>();
            if (view.ViewPool != null)
            {
                var entityView = view.GameObject.GetComponent<EntityView>();
                entityView.Entity = null;
                view.ViewPool.ReleaseInstance(view.GameObject);
            }
            else
            {
                //todo: if entity not pooled - disable\or remove
                Object.Destroy(view.GameObject.GetComponent<EntityView>());
            }
        }
    }
}
