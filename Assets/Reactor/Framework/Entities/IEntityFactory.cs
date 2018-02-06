using System.Collections.Generic;
using Reactor.Components;
using Reactor.Groups;
using Reactor.Pools;

namespace Reactor.Entities
{
    public interface IEntityFactory : Zenject.IFactory<IPool, int, IEnumerable<IComponent>, SystemReactor,IEntity> {}
}