using System;
using Assets.Reactor.Examples.GroupFilters.Components;
using Assets.Reactor.Examples.GroupFilters.Groups;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using UniRx;
using Random = UnityEngine.Random;

namespace Assets.Reactor.Examples.GroupFilters.Systems
{
    public class RandomScoreIncrementerSystem : IEntityReactionSystem
    {
        public IGroup TargetGroup { get { return new HasScoreGroup(); } }

        public IObservable<IEntity> Impact(IEntity entity)
        { return Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => entity); }

        public void Reaction(IEntity entity)
        {
            var scoreComponent = entity.GetComponent<HasScoreComponent>();
            var randomIncrement = Random.Range(1, 5);
            scoreComponent.Score.Value += randomIncrement;
        }
    }
}