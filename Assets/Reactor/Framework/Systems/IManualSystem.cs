using Reactor.Groups;

namespace Reactor.Systems
{
    public interface IManualSystem : ISystem
    {
        void StartSystem(IGroupAccessor group);
        void StopSystem(IGroupAccessor group);
    }
}