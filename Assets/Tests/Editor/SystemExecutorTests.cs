using System;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems;
using Reactor.Systems.Executor;
using Reactor.Systems.Executor.Handlers;
using Reactor.Tests.Components;
using NSubstitute;
using NUnit.Framework;

namespace Reactor.Tests
{
    [TestFixture]
    public class SystemExecutorTests
    {
        [Test]
        public void should_identify_as_setup_system_and_add_to_systems()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockSetupSystemHandler = Substitute.For<ISetupSystemHandler>();
            var fakeSystem = Substitute.For<ISetupSystem>();

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                null, null, mockSetupSystemHandler, null, null);

            systemExecutor.AddSystem(fakeSystem);

            mockSetupSystemHandler.Received().Setup(fakeSystem);
            Assert.That(systemExecutor.Systems, Contains.Item(fakeSystem));
        }

        [Test]
        public void should_identify_as_reactive_entity_system_and_add_to_systems()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockReactToEntitySystemHandler = Substitute.For<IEntityReactionSystemHandler>();
            var fakeSystem = Substitute.For<IEntityReactionSystem>();

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                mockReactToEntitySystemHandler, null, null, null, null);

            systemExecutor.AddSystem(fakeSystem);

            mockReactToEntitySystemHandler.Received().Setup(fakeSystem);
            Assert.That(systemExecutor.Systems, Contains.Item(fakeSystem));
        }

        [Test]
        public void should_identify_as_reactive_group_system_and_add_to_systems()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockReactToGroupSystemHandler = Substitute.For<IGroupReactionSystemHandler>();
            var fakeSystem = Substitute.For<IGroupReactionSystem>();

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                null, mockReactToGroupSystemHandler, null, null, null);

            systemExecutor.AddSystem(fakeSystem);

            mockReactToGroupSystemHandler.Received().Setup(fakeSystem);
            Assert.That(systemExecutor.Systems, Contains.Item(fakeSystem));
        }

        [Test]
        public void should_remove_system_from_systems()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockSetupSystemHandler = Substitute.For<ISetupSystemHandler>();
            var fakeSystem = Substitute.For<ISetupSystem>();

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                null, null, mockSetupSystemHandler, null, null);

            systemExecutor.AddSystem(fakeSystem);
            systemExecutor.RemoveSystem(fakeSystem);

            Assert.That(systemExecutor.Systems, Is.Empty);
        }


        [Test]
        public void should_effect_correct_setup_systems_once()
        {
            var dummyGroup = new Group(typeof(TestComponentOne), typeof(TestComponentTwo));
            var mockPoolManager = Substitute.For<IPoolManager>();
            var pool = mockPoolManager.GetPool();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockSetupSystemHandler = Substitute.For<ISetupSystemHandler>();
            var fakeSystem = Substitute.For<ISetupSystem>();
            var entityIndexPool = Substitute.For<IEntityIndexPool>();
            fakeSystem.TargetGroup.Returns(dummyGroup);

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                null, null, mockSetupSystemHandler, null, null);

            systemExecutor.AddSystem(fakeSystem);

            var entity = new Entity(entityIndexPool.GetId(), pool, mockEventSystem);
            entity.AddComponent(new TestComponentOne());
            systemExecutor.OnEntityComponentAdded(new ComponentAddedEvent(entity, new TestComponentOne()));
            
            entity.AddComponent(new TestComponentTwo());
            systemExecutor.OnEntityComponentAdded(new ComponentAddedEvent(entity, new TestComponentTwo()));

            entity.AddComponent(new TestComponentThree());
            systemExecutor.OnEntityComponentAdded(new ComponentAddedEvent(entity, new TestComponentThree()));

            mockSetupSystemHandler.Received(1).ProcessEntity(Arg.Is(fakeSystem), Arg.Is(entity));
        }

        [Test]
        public void should_only_trigger_teardown_system_when_entity_loses_required_component()
        {
            var dummyGroup = new Group(typeof(TestComponentOne), typeof(TestComponentTwo));
            var mockPoolManager = Substitute.For<IPoolManager>();
            var pool = mockPoolManager.GetPool();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var fakeSystem = Substitute.For<ITeardownSystem>();
            var entityIndexPool = Substitute.For<IEntityIndexPool>();
            fakeSystem.TargetGroup.Returns(dummyGroup);

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                null, null, null, null, null);

            systemExecutor.AddSystem(fakeSystem);

            var entity = new Entity(entityIndexPool.GetId(), pool, mockEventSystem);
            entity.AddComponent(new TestComponentOne());
            entity.AddComponent(new TestComponentTwo());
            
            // Should not trigger
            systemExecutor.OnEntityComponentRemoved(new ComponentRemovedEvent(entity, new TestComponentThree()));

            // Should trigger
            systemExecutor.OnEntityComponentRemoved(new ComponentRemovedEvent(entity, new TestComponentTwo()));

            fakeSystem.Received(1).Teardown(Arg.Is(entity));
        }
    }
}