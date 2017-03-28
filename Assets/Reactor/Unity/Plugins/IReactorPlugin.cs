using System;
using System.Collections.Generic;
using Reactor.Systems;
using Reactor.Systems.Executor;
using Zenject;

namespace Reactor.Unity.Plugins
{
    public interface IReactorPlugin
    {
        string Name { get; }
        Version Version { get; }

        void SetupDependencies(DiContainer container);
        IEnumerable<ISystem> GetSystemForRegistration(DiContainer container);
    }
}