using System.Collections.Generic;
using System.Linq;
using Reactor.Extensions;
using UnityEngine;
using Reactor.Pools;
using Reactor.Systems;
using Reactor.Systems.Executor;
using Reactor.Unity.Plugins;
using Reactor.Unity.Systems;
using Zenject;

namespace Reactor.Unity
{
    public abstract class ReactorApplication : MonoBehaviour
    {
        public ISystemExecutor SystemExecutor { get; private set; }

        [Inject]
        public IPoolManager PoolManager { get; private set; }

        protected List<IReactorPlugin> Plugins { get; private set; }

        protected DiContainer Container { get; private set; }

        [Inject]
        private void Init(DiContainer container, ISystemExecutor systemExecutor, ICoreManager coreManager, IPoolManager poolManager)
        {
            Plugins = new List<IReactorPlugin>();
            SystemExecutor = systemExecutor;
            PoolManager = poolManager;
            //CoreManager = coreManager;
            Container = container;
            ApplicationStarting();
            RegisterAllPluginDependencies();
            SetupAllPluginSystems();
            ApplicationStarted();
        }

        protected virtual void ApplicationStarting() { }
        protected abstract void ApplicationStarted();

        protected virtual void RegisterAllPluginDependencies()
        {
            Plugins.ForEachRun(x => x.SetupDependencies(Container));
        }

        protected virtual void SetupAllPluginSystems()
        {
            Plugins.SelectMany(x => x.GetSystemForRegistration(Container))
                .ForEachRun(x => SystemExecutor.AddSystem(x));
        }

        protected void RegisterPlugin(IReactorPlugin plugin)
        {
            Plugins.Add(plugin);
        }
        
        protected virtual void RegisterAllBoundSystems()
        {
            Debug.Log("RegisterAllBoundSystems");
            var allSystems = Container.ResolveAll<ISystem>();

            var orderedSystems = allSystems
                .OrderByDescending(x => x is ViewResolverSystem)
                .ThenByDescending(x => x is ISetupSystem);

            orderedSystems.ForEachRun(SystemExecutor.AddSystem);
        }

        protected virtual void RegisterBoundSystem<T>() where T : ISystem
        {
            var system = Container.Resolve<T>();
            SystemExecutor.AddSystem(system);
        }
    }
}
