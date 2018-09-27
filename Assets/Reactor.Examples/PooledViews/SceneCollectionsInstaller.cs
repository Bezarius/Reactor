using Assets.Game.SceneCollections;
using Assets.Reactor.Examples.PooledViews.ViewResolvers;
using Zenject;

public class SceneCollectionsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<PrefabLoader<DestructableTypes>>()
            .FromInstance(new PrefabLoader<DestructableTypes>("Destructables/"))
            .AsSingle();
    }
}
