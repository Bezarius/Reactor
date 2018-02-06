using System;
using System.Linq;
using Reactor.Entities;
using Reactor.Groups;
using UniRx;

namespace Reactor.Extensions
{
    public static class IEntityExtensions
    {
        public static IObservable<IEntity> ObserveProperty<T>(this IEntity entity, Func<IEntity, T> propertyLocator)
        {
            return Observable.EveryUpdate()
                .DistinctUntilChanged(x => propertyLocator(entity))
                .Select(x => entity);
        }

        public static IObservable<IEntity> WaitForPredicateMet(this IEntity entity, Predicate<IEntity> predicate)
        {
            return Observable.EveryUpdate()
                .First(x => predicate(entity))
                .Select(x => entity);
        }

        public static bool MatchesGroup(this IEntity entity, IGroup group)
        {
            return entity.HasComponents(group.TargettedComponents.ToArray());
        }
    }
}