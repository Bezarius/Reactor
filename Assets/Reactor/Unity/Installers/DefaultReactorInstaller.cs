using Reactor.Entities;
using Reactor.Events;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems.Executor;
using Reactor.Systems.Executor.Handlers;
using Reactor.Unity.Systems;
using UniRx;
using Zenject;

namespace Reactor.Unity.Installers
{
    public class DefaultReactorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMessageBroker>().To<MessageBroker>().AsSingle();
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

            Container.Bind<ISystemExecutor>().To<SystemExecutor>().AsSingle().NonLazy();
        }
    }
}