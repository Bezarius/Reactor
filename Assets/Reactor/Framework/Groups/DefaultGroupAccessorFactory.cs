using Reactor.Events;

namespace Reactor.Groups
{
    public class DefaultGroupAccessorFactory : IGroupAccessorFactory
    {
        private readonly IEventSystem _eventSystem;

        public DefaultGroupAccessorFactory(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public IGroupAccessor Create(GroupAccessorConfiguration arg)
        {
            var groupAccessor = new CacheableGroupAccessor(arg.GroupAccessorToken, arg.InitialEntities);
            return groupAccessor;            
        }
    }
}