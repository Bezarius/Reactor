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
    public class ReactToGroupSystemHandlerTests
    {
        [Test]
        public void should_return_valid_subscription_token_when_processing()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockSystem = Substitute.For<IGroupReactionSystem>();
            var mockSubscription = Substitute.For<IObservable<IGroupAccessor>>();
            mockSystem.Impact(Arg.Any<IGroupAccessor>()).Returns(mockSubscription);

            var handler = new GroupReactionSystemHandler(mockPoolManager);
            var subscriptionToken = handler.Setup(mockSystem);

            Assert.That(subscriptionToken, Is.Not.Null);
            //Assert.That(subscriptionToken.AssociatedObject, Is.Null);
            //Assert.That(subscriptionToken.Disposable, Is.Not.Null);
        }
    }
}