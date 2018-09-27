using Assets.Reactor.Examples.GroupFilters.Blueprints;
using Reactor.Unity;

namespace Assets.Reactor.Examples.GroupFilters
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

            var entityCount = 1000;

            for (var i = 0; i < entityCount; i++)
            {
                defaultPool.CreateEntity(new PlayerBlueprint("Player " + i, 0));
            }
        }
    }
}