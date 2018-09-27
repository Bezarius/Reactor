using Reactor.Components;

namespace Assets.Reactor.Examples.UsingBlueprints.Components
{
    public class HasName : EntityComponent<HasName>
    {
        public string Name { get; set; }
    }
}