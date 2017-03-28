using Reactor.Unity;

namespace Assets.Reactor.Examples.PooledViews
{
    public class Application : ReactorApplication
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {}
    }
}
