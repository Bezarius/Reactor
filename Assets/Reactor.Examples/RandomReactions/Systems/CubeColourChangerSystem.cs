using Assets.Reactor.Examples.RandomReactions.Components;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Unity.Components;
using UniRx;
using UnityEngine;

namespace Assets.Reactor.Examples.RandomReactions.Systems
{
    public class CubeColourChangerSystem : IEntityReactionSystem
    {
        public IGroup TargetGroup
        {
            get
            {
                return new GroupBuilder()
                    .WithComponent<ViewComponent>()
                    .WithComponent<RandomColorComponent>()
                    .Build();
            }
        }

        public IObservable<IEntity> Impact(IEntity entity)
        {
            var colorComponent = entity.GetComponent<RandomColorComponent>();
            return colorComponent.Color.DistinctUntilChanged().Select(x => entity);
        }

        public void Reaction(IEntity entity)
        {
            var colorComponent = entity.GetComponent<RandomColorComponent>();
            var cubeComponent = entity.GetComponent<ViewComponent>();
            var renderer = cubeComponent.GameObject.GetComponent<Renderer>();
            renderer.material.color = colorComponent.Color.Value;
        }
    }
}