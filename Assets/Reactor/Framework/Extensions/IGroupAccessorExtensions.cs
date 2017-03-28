using System.Collections.Generic;
using Assets.Reactor.Framework.Groups;
using Reactor.Entities;
using Reactor.Groups;

namespace Reactor.Extensions
{
    public static class IGroupAccessorExtensions
    {
        public static IEnumerable<IEntity> Query(this IGroupAccessor groupAccesssor, IGroupAccessorQuery query)
        { return query.Execute(groupAccesssor); }
    }
}