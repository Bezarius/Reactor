using Reactor.Entities;
using Reactor.Groups;
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

        public ColliderInteractionSystem()
        {
            TargetGroup = new GroupBuilder()
                    .WithComponent<SelfDestructComponent>()
                    .WithComponent<ColliderComponent>()
                    .WithComponent<ViewComponent>()
                    .Build();
        }

        public IObservable<IEntity> Impact(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();

            return viewComponent.GameObject
                .OnCollisionEnterAsObservable().Where(x => x.gameObject.CompareTag("IEntity"))
                .Select(x => x.gameObject.GetComponent<EntityView>().Entity);
        }

        public void Reaction(IEntity sourceEntity, IEntity targetEntity)
        {
            if (targetEntity != null)
                targetEntity.Destory();
        }
    }
}