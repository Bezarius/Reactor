using Reactor.Entities;

namespace Reactor.Systems
{
    public interface ISetupSystem : ISystem
    {
        void Setup(IEntity entity);
    }
}