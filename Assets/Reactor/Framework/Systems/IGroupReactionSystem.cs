using System;
using Reactor.Entities;
using Reactor.Groups;

namespace Reactor.Systems
{
    public interface IGroupReactionSystem : ISystem
    {
        IObservable<IGroupAccessor> Impact(IGroupAccessor group);
        void Reaction(IEntity entity);
    }
}