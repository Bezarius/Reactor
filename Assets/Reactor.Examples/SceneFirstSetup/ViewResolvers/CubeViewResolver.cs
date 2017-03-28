using Assets.Reactor.Examples.SceneFirstSetup.Components;
using Reactor.Entities;
using Reactor.Extensions;
using Reactor.Groups;
using Reactor.Unity.Systems;
using UnityEngine;

namespace Assets.Reactor.Examples.SceneFirstSetup.ViewResolvers
{
    public class CubeViewResolver : ViewResolverSystem
    {
        private readonly Transform ParentTrasform = GameObject.Find("Entities").transform;

        public override IGroup TargetGroup
        {
            get { return base.TargetGroup.WithComponent<CubeComponent>(); }
        }

        public CubeViewResolver(IViewHandler viewHandler) : base(viewHandler)
        {}

        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.transform.position = new Vector3(-2, 0, 0);
            view.transform.parent = ParentTrasform;
            return view;
        }
    }
}
