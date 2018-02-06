using System.Collections.Generic;
using Reactor.Blueprints;
using Reactor.Components;
using Reactor.Entities;

namespace Reactor.Pools
{
    public interface IPool
    {
        string Name { get; }
        IEnumerable<IEntity> Entities { get; }
        IEntity BuildEntity<T>(T blueprint) where T : class, IBlueprint;
        IEntity CreateEntity();
        IEntity CreateEntity(IEnumerable<IComponent> components);
        void RemoveEntity(IEntity entity);
    }
}
