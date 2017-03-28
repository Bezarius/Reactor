using Assets.Reactor.Examples.PluginExample.HelloWorldPlugin.components;
using Reactor.Unity;

namespace Assets.Reactor.Examples.PluginExample
{
    public class Application : ReactorApplication
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
            RegisterPlugin(new HelloWorldPlugin.HelloWorldPlugin());
        }
        
        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();
            var entity = defaultPool.CreateEntity();
            entity.AddComponent<SayHelloWorldComponent>();
        }
    }
}