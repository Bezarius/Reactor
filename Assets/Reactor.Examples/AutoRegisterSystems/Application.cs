using Reactor.Unity;
using Reactor.Unity.Components;

namespace Assets.Reactor.Examples.AutoRegisterSystems
{
    public class Application : ReactorApplication
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();
            var entity = defaultPool.CreateEntity();
            entity.AddComponent(new ViewComponent());
        }
    }
}