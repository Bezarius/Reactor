using Reactor.Groups;
using Reactor.Pools;

namespace Reactor.Events
{
    public class GroupAccessorAddedEvent
    {
        public IGroupAccessor GroupAccessor { get; }
        public GroupAccessorToken GroupAccessorToken { get; }

        public GroupAccessorAddedEvent(GroupAccessorToken groupAccessorToken, IGroupAccessor groupAccessor)
        {
            GroupAccessor = groupAccessor;
            GroupAccessorToken = groupAccessorToken;
        }
    }
}