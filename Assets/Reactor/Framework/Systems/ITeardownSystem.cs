using Reactor.Entities;

namespace Reactor.Systems
{
    public interface ITeardownSystem : ISystem
    {
        void Teardown(IEntity entity);
    }
}