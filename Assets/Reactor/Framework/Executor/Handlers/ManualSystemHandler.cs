using System.Linq;
using Reactor.Pools;

namespace Reactor.Systems.Executor.Handlers
{
    public class ManualSystemHandler : IManualSystemHandler
    {
        public IPoolManager PoolManager { get; }

        public ManualSystemHandler(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public void Start(IManualSystem system)
        {
            if (system.TargetGroup.TargettedComponents.ToArray().Length > 0)
            {
                var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
                system.StartSystem(groupAccessor);
            }
            else
            {
                system.StartSystem(null);
            }
        }

        public void Stop(IManualSystem system)
        {
            if (system.TargetGroup.TargettedComponents.ToArray().Length > 0)
            {
                var groupAccessor = PoolManager.CreateGroupAccessor(system.TargetGroup);
                system.StopSystem(groupAccessor);
            }
            else
            {
                system.StopSystem(null);
            }
        }
    }
}