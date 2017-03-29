using Reactor.Entities;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems;
using Reactor.Unity.Components;
using Reactor.Unity.MonoBehaviours;
using UniRx;
using UniRx.Triggers;

namespace Assets.Reactor.Examples.PooledViews.Systems
{
    public class ColliderInteractionSystem : IInteractReactionSystem
    {
        public IGroup TargetGroup { get; private set; }

        private readonly IPool _pool;

        public ColliderInteractionSystem(IPoolManager poolManager)
        {
            TargetGroup = new Group(
                            typeof(SelfDestructComponent),
                            typeof(ColliderComponent),
                            typeof(ViewComponent));

            _pool = poolManager.GetPool();
        }

        public IObservable<IEntity> Impact(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();

            return viewComponent.View
                .OnCollisionEnterAsObservable().Where(x => x.gameObject.CompareTag("IEntity"))
                .Select(x => x.gameObject.GetComponent<EntityView>().Entity);
        }

        public void Reaction(IEntity sourceEntity, IEntity targetEntity)
        {
            if(targetEntity != null)
                _pool.RemoveEntity(targetEntity);
        }
    }
}