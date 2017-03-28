using Reactor.Entities;
using Reactor.Events;
using Reactor.Pools;
using Reactor.Unity.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Reactor.Examples.SimpleMovement.ViewResolvers
{
    public class DefaultViewResolver : ViewResolverSystem
    {
        public DefaultViewResolver(IViewHandler viewHandler) : base(viewHandler)
        {}

        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.name = "entity-" + entity.Id;
            var rigidBody = view.AddComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
            return view;
        }
    }
}