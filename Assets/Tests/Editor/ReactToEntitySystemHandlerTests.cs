using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems;
using Reactor.Systems.Executor.Handlers;
using Reactor.Tests.Components;
using NSubstitute;
using NUnit.Framework;
using UniRx;

namespace Reactor.Tests
{
    [TestFixture]
    public class ReactToEntitySystemHandlerTests
    {
        [Test]
        public void should_return_valid_subscription_token_when_processing()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEntity = Substitute.For<IEntity>();
            var mockSystem = Substitute.For<IEntityReactionSystem>();
            var mockSubscription = Substitute.For<IObservable<IEntity>>();
            mockSystem.Impact(mockEntity).Returns(mockSubscription);

            var handler = new EntityReactionSystemHandler(mockPoolManager);
            var subscriptionToken = handler.ProcessEntity(mockSystem, mockEntity);

            Assert.That(subscriptionToken, Is.Not.Null);
            Assert.That(subscriptionToken.AssociatedObject, Is.EqualTo(mockEntity));
            Assert.That(subscriptionToken.Disposable, Is.Not.Null);
        }

        [Test]
        public void should_return_valid_subscription_collection()
        {
            var dummyGroup = new GroupBuilder().WithComponent<TestComponentOne>().Build();
            var mockEntity = Substitute.For<IEntity>();

            var mockPoolManager = Substitute.For<IPoolManager>();
            mockPoolManager.CreateGroupAccessor(dummyGroup).Returns(new GroupAccessor(null, new[] {mockEntity}));

            var mockSystem = Substitute.For<IEntityReactionSystem>();
            mockSystem.TargetGroup.Returns(dummyGroup);

            var mockSubscription = Substitute.For<IObservable<IEntity>>();
            mockSystem.Impact(mockEntity).Returns(mockSubscription);

            var handler = new EntityReactionSystemHandler(mockPoolManager);

            var subscriptionTokens = handler.Setup(mockSystem);
            Assert.That(subscriptionTokens.Count(), Is.EqualTo(1));
            Assert.That(subscriptionTokens.ElementAt(0).AssociatedObject, Is.EqualTo(mockEntity));
            Assert.That(subscriptionTokens.ElementAt(0).Disposable, Is.Not.Null);
        }
    }
}