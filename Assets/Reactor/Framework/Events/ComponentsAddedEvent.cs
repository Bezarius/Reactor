using System.Collections.Generic;
using Reactor.Components;
using Reactor.Entities;

namespace Reactor.Events
{
    public class ComponentsAddedEvent
    {
        public IEntity Entity { get; private set; }
        public IEnumerable<IComponent> Components { get; private set; }

        public ComponentsAddedEvent(IEntity entity, IEnumerable<IComponent> components)
        {
            Entity = entity;
            Components = components;
        }
    }
}