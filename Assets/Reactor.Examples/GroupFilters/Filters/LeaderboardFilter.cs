using System.Collections.Generic;
using System.Linq;
using Assets.Reactor.Examples.GroupFilters.Components;
using Assets.Reactor.Framework.Groups.Filtration;
using Reactor.Groups;

namespace Assets.Reactor.Examples.GroupFilters.Filters
{
    public class LeaderboardFilter : IGroupAccessorFilter<HasScoreComponent>
    {
        public IGroupAccessor GroupAccessor { get; private set; }

        public LeaderboardFilter(IGroupAccessor groupAccessor)
        { GroupAccessor = groupAccessor; }

        public IEnumerable<HasScoreComponent> Filter()
        {
            return GroupAccessor.Entities
                .Select(x => x.GetComponent<HasScoreComponent>())
                .OrderByDescending(x => x.Score.Value)
                .Take(5);
        }
    }
}