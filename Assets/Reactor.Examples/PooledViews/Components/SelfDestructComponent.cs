using Assets.Reactor.Examples.PooledViews.ViewResolvers;
using Reactor.Components;

public class SelfDestructComponent : EntityComponent<SelfDestructComponent>
{
    public DestructableTypes DestructableTypes;
    public float Lifetime;
}