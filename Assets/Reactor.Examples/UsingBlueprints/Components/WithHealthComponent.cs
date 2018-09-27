using Reactor.Components;

namespace Assets.Reactor.Examples.UsingBlueprints.Components
{
    public class WithHealthComponent : EntityComponent<WithHealthComponent>
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
    }
}