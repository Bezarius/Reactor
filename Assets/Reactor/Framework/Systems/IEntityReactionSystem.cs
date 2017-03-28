using Reactor.Entities;
using UniRx;

namespace Reactor.Systems
{
    public interface IEntityReactionSystem : ISystem
    {
        IObservable<IEntity> Impact(IEntity entity);

        void Reaction(IEntity entity);
    }
}