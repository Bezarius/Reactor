using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Extensions;
using Reactor.Pools;
using UniRx;

namespace Reactor.Systems.Executor.Handlers
{
    public class ManualSystemHandler : IManualSystemHandler
    {
        public IPoolManager PoolManager { get; private set; }

        public ManualSystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public void Start(IManualSystem system)
        {
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            system.StartSystem(groupAccessor);
        }

        public void Stop(IManualSystem system)
        {
            var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
            system.StopSystem(groupAccessor);
        }
    }
}