using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Extensions;
using Reactor.Groups;
using UnityEngine.Assertions;

namespace Reactor.Pools
{
    public class PoolManager : IPoolManager, IDisposable
    {
        public const string DefaultPoolName = "default";

        private readonly IDictionary<GroupAccessorToken, IGroupAccessor> _groupAccessors;
        private readonly IDictionary<string, IPool> _pools;

        public IEventSystem EventSystem { get; }
        public IEnumerable<IPool> Pools => _pools.Values;
        public IPoolFactory PoolFactory { get; }
        public IGroupAccessorFactory GroupAccessorFactory { get; }

        public PoolManager(IEventSystem eventSystem, IPoolFactory poolFactory, IGroupAccessorFactory groupAccessorFactory)
        {
            EventSystem = eventSystem;
            PoolFactory = poolFactory;
            GroupAccessorFactory = groupAccessorFactory;

            _groupAccessors = new Dictionary<GroupAccessorToken, IGroupAccessor>();
            _pools = new Dictionary<string, IPool>();

            CreatePool(DefaultPoolName);
        }

        public IPool CreatePool(string name)
        {
            var pool = PoolFactory.Create(name);
            _pools.Add(name, pool);

            EventSystem.Publish(new PoolAddedEvent(pool));

            return pool;
        }

        public IPool GetPool(string poolName = null)
        {
            IPool pool;
            if (string.IsNullOrEmpty(poolName))
            {
                pool = _pools[DefaultPoolName];
            }
            else
            {
                if (!_pools.TryGetValue(poolName, out pool))
                    pool = CreatePool(poolName);
                    //throw new Exception(string.Format("Try access to non existing pool: '{0}'! Use CreatePool for new pool creation.", poolName));

            }
            return pool;
        }

        public void RemovePool(string name)
        {
            if (!_pools.ContainsKey(name))
            {
                return;
            }

            var pool = _pools[name];
            _pools.Remove(name);

            EventSystem.Publish(new PoolRemovedEvent(pool));
        }

        public IEnumerable<IEntity> GetEntitiesFor(IGroup group, string poolName = null)
        {
            if (group is EmptyGroup)
            {
                return new IEntity[0];
            }
            var result = this.GetPool(poolName).Entities.MatchingGroup(group);

            return result;
        }

        public IGroupAccessor CreateGroupAccessor(IGroup group, string poolName)
        {
            if (string.IsNullOrEmpty(poolName))
                poolName = DefaultPoolName;

            var groupAccessorToken = new GroupAccessorToken(group.TargettedComponents.ToArray(), poolName);
            if (_groupAccessors.ContainsKey(groupAccessorToken))
            {
                return _groupAccessors[groupAccessorToken];
            }

            var entityMatches = GetEntitiesFor(group, poolName);
            var groupAccessor = GroupAccessorFactory.Create(new GroupAccessorConfiguration
            {
                GroupAccessorToken = groupAccessorToken,
                InitialEntities = entityMatches
            });
#if UNITY_EDITOR

            Assert.IsFalse(groupAccessorToken.ComponentTypes.Length == 0, "groupAccessorToken.ComponentTypes.Length == 0");
#endif
            _groupAccessors.Add(groupAccessorToken, groupAccessor);

            EventSystem.Publish(new GroupAccessorAddedEvent(groupAccessorToken, groupAccessor));

            return _groupAccessors[groupAccessorToken];
        }

        public IEnumerable<IGroupAccessor> PoolAccessors
        {
            get
            {
                return _groupAccessors.Values;
            }
        }

        public void Dispose()
        {
            _groupAccessors.Values.ForEachRun(x =>
            {
                var disposable = x as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            });
        }
    }
}