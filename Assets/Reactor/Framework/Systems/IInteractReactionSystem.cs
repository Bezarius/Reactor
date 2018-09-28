using System;
using Reactor.Entities;

namespace Reactor.Systems
{
    public interface IInteractReactionSystem : ISystem
    {
        IObservable<IEntity> Impact(IEntity entity);

        void Reaction(IEntity sourceEntity, IEntity targetEntity);
    }
}