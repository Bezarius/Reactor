using Assets.Reactor.Examples.ManuallyRegisterSystems.Systems;
using Reactor.Unity;
using Reactor.Unity.Components;
using Zenject;

namespace Assets.Reactor.Examples.ManuallyRegisterSystems
{
    public class Application : ReactorApplication
    {
        protected override void ApplicationStarting()
        {
            RegisterBoundSystem<DefaultViewResolver>();
            RegisterBoundSystem<RandomMovementSystem>();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();
            
            var entity = defaultPool.CreateEntity();
            entity.AddComponent(new ViewComponent());
        }
    }
}