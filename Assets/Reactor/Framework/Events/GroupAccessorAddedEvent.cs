using Reactor.Groups;
using Reactor.Pools;

namespace Reactor.Events
{
    public class GroupAccessorAddedEvent
    {
        public IGroupAccessor GroupAccessor { get; private set; }
        public GroupAccessorToken GroupAccessorToken { get; private set; }

        public GroupAccessorAddedEvent(GroupAccessorToken groupAccessorToken, IGroupAccessor groupAccessor)
        {
            GroupAccessor = groupAccessor;
            GroupAccessorToken = groupAccessorToken;
        }
    }
}