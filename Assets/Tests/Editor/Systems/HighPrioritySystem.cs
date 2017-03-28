using Reactor.Attributes;
using Reactor.Groups;
using Reactor.Systems;

namespace Reactor.Tests.Systems
{
    [Priority(1)]
    public class HighPrioritySystem : ISystem
    {
        public IGroup TargetGroup { get { return null; } }
    }
}