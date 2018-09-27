using Assets.Reactor.Examples.PooledViews.ViewResolvers;
using Zenject;

public class AppInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IDestructableFactory>().To<DestructableFactory>().AsSingle();
    }
}