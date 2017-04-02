using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Extensions;
using Reactor.Pools;
using UniRx;

namespace Reactor.Groups
{
    public class CacheableGroupAccessor : IGroupAccessor, IDisposable
    {
        public readonly HashSet<IEntity> CachedEntities;
        public readonly IList<IDisposable> Subscriptions;

        public GroupAccessorToken AccessorToken { get; private set; }
        public IEnumerable<IEntity> Entities { get { return CachedEntities; } }
        public IEventSystem EventSystem { get; private set; }

        public CacheableGroupAccessor(GroupAccessorToken accessorToken, IEnumerable<IEntity> initialEntities, IEventSystem eventSystem)
        {
            AccessorToken = accessorToken;
            EventSystem = eventSystem;

            CachedEntities = new HashSet<IEntity>(initialEntities);
            Subscriptions = new List<IDisposable>();
        }

        public void MonitorEntityChanges()
        {
            var addEntitySubscription = EventSystem.Receive<EntityAddedEvent>()
                .Subscribe(OnEntityAddedToPool);

            var removeEntitySubscription = EventSystem.Receive<EntityRemovedEvent>()
                .Where(x => CachedEntities.Contains(x.Entity))
                .Subscribe(OnEntityRemovedFromPool);

            var addComponentSubscription = EventSystem.Receive<ComponentAddedEvent>()
                .Subscribe(OnEntityComponentAdded);

            var removeComponentEntitySubscription = EventSystem.Receive<ComponentRemovedEvent>()
                .Where(x => CachedEntities.Contains(x.Entity))
                .Subscribe(OnEntityComponentRemoved);

            Subscriptions.Add(addEntitySubscription);
            Subscriptions.Add(removeEntitySubscription);
            Subscriptions.Add(addComponentSubscription);
            Subscriptions.Add(removeComponentEntitySubscription);
        }

        public void OnEntityComponentRemoved(ComponentRemovedEvent args)
        {
            if (CachedEntities.Contains(args.Entity) && !args.Entity.HasComponents(AccessorToken.ComponentTypes))
            {
                CachedEntities.Remove(args.Entity);
            }
        }

        public void OnEntityComponentAdded(ComponentAddedEvent args)
        {
            if (!CachedEntities.Contains(args.Entity) && args.Entity.HasComponents(AccessorToken.ComponentTypes))
            {
                CachedEntities.Add(args.Entity);
            }
        }

        public void OnEntityAddedToPool(EntityAddedEvent args)
        {
            if (!args.Entity.Components.Any()) { return; }
            if (!args.Entity.HasComponents(AccessorToken.ComponentTypes)) { return; }
            CachedEntities.Add(args.Entity);
        }

        public void OnEntityRemovedFromPool(EntityRemovedEvent args)
        {
            if (CachedEntities.Contains(args.Entity))
            {
                CachedEntities.Remove(args.Entity);
            }
        }

        public void Dispose()
        {
            Subscriptions.DisposeAll();
        }
    }
}