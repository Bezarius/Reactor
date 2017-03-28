using Assets.Reactor.Examples.GameObjectBinding.components;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Extensions;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Unity.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Reactor.Examples.GameObjectBinding
{
    public class SphereViewResolver : ViewResolverSystem
    {
        public override IGroup TargetGroup
        {
            get { return base.TargetGroup.WithComponent<SphereComponent>(); }
        }

        public SphereViewResolver(IViewHandler viewHandler) : base(viewHandler)
        {}
        
        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            view.transform.position = new Vector3(2,0,0);
            return view;
        }
    }
}