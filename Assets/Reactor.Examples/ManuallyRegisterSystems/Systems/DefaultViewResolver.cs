using Reactor.Entities;
using Reactor.Events;
using Reactor.Pools;
using Reactor.Unity.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Reactor.Examples.ManuallyRegisterSystems.Systems
{
    public class DefaultViewResolver : ViewResolverSystem
    {
        public DefaultViewResolver(IViewHandler viewHandler) : base(viewHandler)
        {}

        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.name = "entity-" + entity.Id;
            return view;
        }
    }
}