using System;
using System.Collections.Generic;
using Assets.Reactor.Examples.PluginExample.HelloWorldPlugin.systems;
using Reactor.Systems;
using Reactor.Unity.Plugins;
using Zenject;

namespace Assets.Reactor.Examples.PluginExample.HelloWorldPlugin
{
    public class HelloWorldPlugin : IReactorPlugin
    {
        public string Name { get { return "Hello World Plugin";  } }
        public Version Version { get { return new Version(1,0,0); } }

        public void SetupDependencies(DiContainer container)
        {
            container.Bind<OutputHelloWorldSystem>().AsSingle();
        }

        public IEnumerable<ISystem> GetSystemForRegistration(DiContainer container)
        {
            return new[]
            {
                container.Resolve<OutputHelloWorldSystem>()
            };
        }
    }
}