using System;
using System.Collections.Generic;
using Reactor.Components;
using Reactor.Entities;
using Reactor.Extensions;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems.Executor;
using UniRx;

namespace Reactor.Systems
{
    public interface IEntityReactionSystem : ISystem
    {
        IObservable<IEntity> Impact(IEntity entity);

        void Reaction(IEntity entity);
    }
}