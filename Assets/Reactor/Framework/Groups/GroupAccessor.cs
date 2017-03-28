using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Groups
{
    public class GroupAccessor : IGroupAccessor
    {
        public GroupAccessorToken AccessorToken { get; private set; }
        public IEnumerable<IEntity> Entities { get; private set; }

        public GroupAccessor(GroupAccessorToken accessorToken, IEnumerable<IEntity> entities)
        {
            AccessorToken = accessorToken;
            Entities = entities;
        }
    }
}