using Assets.Reactor.Examples.UsingBlueprints.Blueprints;
using Reactor.Unity;

namespace Assets.Reactor.Examples.UsingBlueprints
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

            defaultPool.CreateEntity(new PlayerBlueprint("Player One"));
            defaultPool.CreateEntity(new PlayerBlueprint("Player Two", 150.0f));
        }
    }
}