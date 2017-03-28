using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using Reactor.Tests.Components;

namespace Reactor.Tests.Systems
{
    public class TestSetupSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new Group(typeof(TestComponentOne));} }

        public void Setup(IEntity entity)
        {
            var testComponent = entity.GetComponent<TestComponentOne>();
            testComponent.Data = "woop";
        }
    }
}