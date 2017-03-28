using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Groups
{
    public class GroupAccessorConfiguration
    {
        public GroupAccessorToken GroupAccessorToken { get; set; }
        public IEnumerable<IEntity> InitialEntities { get; set; }
    }
}