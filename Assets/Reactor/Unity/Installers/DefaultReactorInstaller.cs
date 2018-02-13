using System;
using System.Linq;
using System.Reflection;
using Reactor.Components;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems.Executor;
using Reactor.Systems.Executor.Handlers;
using Reactor.Unity.Systems;
using UniRx;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.Installers
{
    public class DefaultReactorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InitEntityComponents();

            Container.Bind<IMessageBroker>().To<MessageBroker>().AsSingle();
            Container.Bind<ISystemExecutor>().To<SystemExecutor>().AsSingle();
            Container.Bind<IEventSystem>().To<EventSystem>().AsSingle();

            Container.Bind<IEntityIndexPool>().To<EntityIndexPool>().AsSingle();
            Container.Bind<IEntityFactory>().To<DefaultEntityFactory>().AsSingle();
            Container.Bind<IPoolFactory>().To<DefaultPoolFactory>().AsSingle();
            Container.Bind<IGroupAccessorFactory>().To<DefaultGroupAccessorFactory>().AsSingle();

            Container.Bind<IPoolManager>().To<PoolManager>().AsSingle();
            Container.Bind<IViewHandler>().To<ViewHandler>().AsSingle();

            Container.Bind<IInteractReactionSystemHandler>().To<InteractReactionSystemHandler>();
            Container.Bind<IEntityReactionSystemHandler>().To<EntityReactionSystemHandler>();
            Container.Bind<IGroupReactionSystemHandler>().To<GroupReactionSystemHandler>();
            Container.Bind<ISetupSystemHandler>().To<SetupSystemHandler>();
            Container.Bind<IManualSystemHandler>().To<ManualSystemHandler>();

            Container.Bind<ISystemHandlerManager>().To<SystemHandlerManager>();

            Container.Bind<ICoreManager>().To<CoreManager>().NonLazy();

            Container.Bind<IUnitFactory>().To<UnitFactory>().AsSingle();
            Container.Bind<IMissileFactory>().To<MissileFactory>().AsSingle();
        }

        private void InitEntityComponents()
        {

            // force component initialization
            var assignFrom = typeof(EntityComponent<>);

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(t => assignFrom.IsAssignableFrom(t) && !t.IsAbstract).ToList();

            var tt = typeof(TypeCache<>);

            foreach (var type in types)
            {
                var args = new[] { type };
                var cache = tt.MakeGenericType(args);
                var field = cache.GetField("TypeId", BindingFlags.Static | BindingFlags.Public);
                var result = field.GetValue(null);
                Debug.Log(result);
            }
        }
    }
}