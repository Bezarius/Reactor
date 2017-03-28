using System.Collections.Generic;
using Reactor.Blueprints;
using Reactor.Entities;

namespace Reactor.Pools
{
    public interface IPool
    {
        string Name { get; }

        IEnumerable<IEntity> Entities { get; }

        IEntity CreateEntity(IBlueprint blueprint = null);
        void RemoveEntity(IEntity entity);
    }
}
