using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Pools;

namespace Reactor.Extensions
{
    public static class IDictionaryExtensions
    {
        public static void AddEntityToGroups(this IDictionary<IGroup, IList<IEntity>> groupAccessors, IEntity entity)
        {
            foreach (var group in groupAccessors.Keys)
            {
                if(entity.MatchesGroup(group))
                { groupAccessors[group].Add(entity); }
            }
        }
    }
}