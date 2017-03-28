using System.Collections.Generic;
using Reactor.Components;
using Reactor.Entities;

namespace Reactor.Events
{
    public class ComponentsRemovedEvent
    {
        public IEntity Entity { get; private set; }
        public IEnumerable<IComponent> Components { get; private set; }

        public ComponentsRemovedEvent(IEntity entity, IEnumerable<IComponent> components)
        {
            Entity = entity;
            Components = components;
        }
    }
}