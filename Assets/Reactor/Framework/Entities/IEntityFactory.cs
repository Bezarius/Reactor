using System;
using Reactor.Factories;
using Reactor.Pools;
using Zenject;

namespace Reactor.Entities
{
    public interface IEntityFactory : IFactory<IPool, Guid?, IEntity> {}
}