using Reactor.Entities;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems;
using Reactor.Systems.Executor.Handlers;
using NSubstitute;
using NUnit.Framework;
using UniRx;

namespace Reactor.Tests
{
    [TestFixture]
    public class SetupSystemHandlerTests
    {
        [Test]
        public void should_return_valid_subscription_token_when_processing()
        {
            var mockEnity = Substitute.For<IEntity>();
            var mockPoolManager = Substitute.For<IPoolManager>();
            var fakeGroupAccessor = new GroupAccessor(null, new [] {mockEnity});
            mockPoolManager.CreateGroupAccessor(Arg.Any<IGroup>()).Returns(fakeGroupAccessor);
            var mockSystem = Substitute.For<ISetupSystem>();

            var handler = new SetupSystemHandler(mockPoolManager);
            handler.Setup(mockSystem);

            mockSystem.Received().Setup(mockEnity);
        }
    }
}