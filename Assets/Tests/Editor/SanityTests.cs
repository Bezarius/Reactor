using Reactor.Entities;
using Reactor.Events;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems.Executor;
using Reactor.Systems.Executor.Handlers;
using Reactor.Tests.Components;
using Reactor.Tests.Systems;
using NUnit.Framework;
using UniRx;

namespace Reactor.Tests
{
    [TestFixture]
    public class SanityTests
    {
        private SystemExecutor CreateExecutor()
        {
            var messageBroker = new EventSystem(new MessageBroker());
            var entityFactory = new DefaultEntityFactory(messageBroker);
            var poolFactory = new DefaultPoolFactory(entityFactory, messageBroker);
            var groupAccessorFactory = new DefaultGroupAccessorFactory(messageBroker);
            var poolManager = new PoolManager(messageBroker, poolFactory, groupAccessorFactory);
            var reactsToEntityHandler = new EntityReactionSystemHandler(poolManager);
            var reactsToGroupHandler = new GroupReactionSystemHandler(poolManager);
            var reactToComponentHandler = new InteractReactionSystemHandler(poolManager);
            var manualSystemHandler = new ManualSystemHandler(poolManager);
            var setupHandler = new SetupSystemHandler(poolManager);
            return new SystemExecutor(poolManager, messageBroker, reactsToEntityHandler, 
                reactsToGroupHandler, setupHandler, reactToComponentHandler, manualSystemHandler);
        }

        [Test]
        public void should_execute_setup_for_matching_entities()
        {
            var executor = CreateExecutor();
            executor.AddSystem(new TestSetupSystem());

            var defaultPool = executor.PoolManager.GetPool();
            var entityOne = defaultPool.CreateEntity();
            var entityTwo = defaultPool.CreateEntity();

            entityOne.AddComponent(new TestComponentOne());
            entityTwo.AddComponent(new TestComponentTwo());

            Assert.That(entityOne.GetComponent<TestComponentOne>().Data, Is.EqualTo("woop"));
            Assert.That(entityTwo.GetComponent<TestComponentTwo>().Data, Is.Null);
        }
    }
}