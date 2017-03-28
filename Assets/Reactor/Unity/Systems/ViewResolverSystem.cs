using Reactor.Entities;
using Reactor.Events;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems;
using Reactor.Unity.Components;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.Systems
{
    public abstract class ViewResolverSystem : ISetupSystem
    {
        public IViewHandler ViewHandler { get; private set; }

        public virtual IGroup TargetGroup
        {
            get { return new Group(typeof(ViewComponent)); }
        }

        protected ViewResolverSystem(IViewHandler viewHandler)
        {
            ViewHandler = viewHandler;
        }

        public abstract GameObject ResolveView(IEntity entity);

        public void Setup(IEntity entity)
        {
            ViewHandler.SetupView(entity, ResolveView);
        }
    }
}