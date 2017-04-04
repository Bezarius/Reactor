using Reactor.Pools;

namespace Reactor.Entities
{
    public interface IEntityFactory : Zenject.IFactory<IPool, int, IEntity> {}
}