using Assets.Reactor.Examples.Performance.Components;
using Reactor.Unity;

namespace Assets.Reactor.Examples.Performance
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

            // create 5k entities
            for (int i = 0; i < 5000; i++)
            {
                var entity = defaultPool.CreateEntity();

                entity.AddComponent<Component1>();
                entity.AddComponent<Component2>();
                entity.AddComponent<Component3>();
            }
        }
    }
}
