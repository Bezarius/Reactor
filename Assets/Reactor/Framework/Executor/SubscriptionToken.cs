using System;
using Reactor.Entities;

namespace Reactor.Systems.Executor
{
    public class SubscriptionToken
    {
        public IEntity AssociatedEntity { get; private set; }
        public IDisposable Disposable { get; private set; }

        public SubscriptionToken(IEntity associatedEntity, IDisposable disposable)
        {
            AssociatedEntity = associatedEntity;
            Disposable = disposable;
        }
    }
}