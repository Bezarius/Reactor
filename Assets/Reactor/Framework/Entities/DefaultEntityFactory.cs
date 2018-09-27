using System.Collections.Generic;
using Reactor.Components;
using Reactor.Groups;
using Reactor.Pools;

namespace Reactor.Entities
{
    // todo: to non-Zenject factory
    public class DefaultEntityFactory : IEntityFactory
    {
        public IEntity Create(IPool pool, int entityId, IEnumerable<IComponent> components, SystemReactor systemReactor)
        {
            return new Entity(entityId, components, pool, systemReactor);
        }
    }
}