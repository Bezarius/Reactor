using Reactor.Components;

namespace Reactor.Tests.Components
{
    public class TestComponentThree : EntityComponent<TestComponentThree>
    {
        public string Data { get; set; }
    }
}