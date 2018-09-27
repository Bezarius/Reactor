using Reactor.Components;

namespace Reactor.Tests.Components
{
    public class TestComponentTwo : EntityComponent<TestComponentTwo>
    {
        public string Data { get; set; }
    }
}