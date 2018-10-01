using Reactor.Components;
using Reactor.Entities;

namespace Reactor.Events
{
    public class ComponentAddedEvent
    {
        public IEntity Entity { get; }
        public IComponent Component { get; }

        public ComponentAddedEvent(IEntity entity, IComponent component)
        {
            Entity = entity;
            Component = component;
        }
    }
}