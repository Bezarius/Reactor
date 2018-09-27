using Reactor.Components;

namespace Reactor.Tests.Components
{
    public class TestComponentOne : EntityComponent<TestComponentOne>
    {
         public string Data { get; set; }
    }
}