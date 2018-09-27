using Reactor.Components;

namespace Assets.Reactor.Examples.SceneFirstSetup.Components
{
    public class TestComponent : EntityComponent<TestComponent>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsHappy { get; set; }
        public float Roundness { get; set; }
    }
}