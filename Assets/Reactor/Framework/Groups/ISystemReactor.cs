using Reactor.Components;
using Reactor.Entities;

namespace Reactor.Groups
{
    public interface ISystemReactor : ISystemContainer
    {
        void AddComponent(IEntity entity, IComponent component);
        void AddEntityToReactor(IEntity entity);
        int GetComponentIdx(int componentId);
        int GetFutureComponentIdx(IComponent component);
        bool HasComponentIndex(int componentId);
        void RemoveComponent(IEntity entity, IComponent component);
    }
}