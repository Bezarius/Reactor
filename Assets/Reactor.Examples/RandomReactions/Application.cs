using Assets.Reactor.Examples.RandomReactions.Components;
using Reactor.Unity;
using Reactor.Unity.Components;

namespace Assets.Reactor.Examples.RandomReactions
{
    public class Application : ReactorApplication
    {
        private readonly int _cubeCount = 500;

        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();

            for (var i = 0; i < _cubeCount; i++)
            {
                var viewEntity = defaultPool.CreateEntity();
                viewEntity.AddComponent(new ViewComponent());
                viewEntity.AddComponent(new RandomColorComponent());
            }
        }
    }
}