using Reactor.Attributes;
using Reactor.Groups;
using Reactor.Systems;

namespace Reactor.Tests.Systems
{
    [Priority(-100)]
    public class LowPrioritySystem : ISystem
    {
        public IGroup TargetGroup { get { return null; } }
    }
}