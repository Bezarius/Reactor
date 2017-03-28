using Reactor.Entities;
using Reactor.Events;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Unity.Components;
using Reactor.Unity.Systems;
using UnityEngine;

namespace Assets.Reactor.Examples.PooledViews.ViewResolvers
{
    public class SelfDestructionViewResolver : DefaultPooledViewResolverSystem
    {
        public override IGroup TargetGroup
        {
            get { return new Group(typeof(SelfDestructComponent), typeof(ViewComponent)); }
        }

        public SelfDestructionViewResolver(IPoolManager poolManager, IEventSystem eventSystem)
            : base(poolManager, eventSystem)
        {
            ViewPool.PreAllocate(20);
        }

        protected override GameObject ResolvePrefabTemplate()
        {
            return Resources.Load("PooledPrefab") as GameObject;
        }

        protected override GameObject AllocateView(IEntity entity)
        {
            var selfDestructComponent = entity.GetComponent<SelfDestructComponent>();
            var allocatedView = base.AllocateView(entity);
            allocatedView.transform.position = selfDestructComponent.StartingPosition;
            return allocatedView;
        }
    }
}