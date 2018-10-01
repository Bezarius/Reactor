using System;
using Reactor.Entities;

namespace Reactor.Systems.Executor
{
    public class SubscriptionToken
    {
        public IEntity AssociatedEntity { get; }
        public IDisposable Disposable { get; }

        public SubscriptionToken(IEntity associatedEntity, IDisposable disposable)
        {
            AssociatedEntity = associatedEntity;
            Disposable = disposable;
        }
    }
}