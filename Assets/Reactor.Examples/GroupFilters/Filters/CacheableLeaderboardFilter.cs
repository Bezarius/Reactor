using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Reactor.Examples.GroupFilters.Components;
using Assets.Reactor.Framework.Groups.Filtration;
using Reactor.Extensions;
using Reactor.Groups;
using UniRx;
using UnityEngine;

namespace Assets.Reactor.Examples.GroupFilters.Filters
{
    public class CacheableLeaderboardFilter : CacheableGroupAccessorFilter<HasScoreComponent>
    {
        public CacheableLeaderboardFilter(IGroupAccessor groupAccessor) : base(groupAccessor)
        {}

        protected override IEnumerable<HasScoreComponent> FilterQuery()
        {
            Debug.Log("Updating");
            return GroupAccessor.Entities
                .Select(x => x.GetComponent<HasScoreComponent>())
                .OrderByDescending(x => x.Score.Value)
                .Take(5)
                .ToList();
        }

        protected override IObservable<Unit> TriggerOnChange()
        {
            return Observable.Interval(TimeSpan.FromSeconds(1)).AsTrigger();
        }
    }
}