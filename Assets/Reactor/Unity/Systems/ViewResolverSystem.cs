using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using UnityEngine;

namespace Reactor.Unity.Systems
{
    public abstract class ViewResolverSystem : ISetupSystem
    {
        public IViewHandler ViewHandler { get; private set; }

        public virtual IGroup TargetGroup { get; protected set; }

        protected ViewResolverSystem(IViewHandler viewHandler)
        {
            TargetGroup = new GroupBuilder()
                .WithComponent<ViewComponent>()
                .Build();

            ViewHandler = viewHandler;
        }

        public abstract GameObject ResolveView(IEntity entity);

        public void Setup(IEntity entity)
        {
            ViewHandler.SetupView(entity, ResolveView);
        }
    }
}