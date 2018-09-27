﻿using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Components;
using Reactor.Entities;
using Reactor.Events;
using Reactor.Pools;
using Reactor.Tests.Components;
using NSubstitute;
using NUnit.Framework;
using UniRx;

namespace Reactor.Tests
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void should_correctly_add_component()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();

            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();

            /*
            var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            var dummyComponent = Substitute.For<IComponent>();

            entity.AddComponent(dummyComponent);

            Assert.That(entity.Components.Count(), Is.EqualTo(1));*/
        }

        [Test]
        public void should_correctly_raise_event_when_adding_component()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();

            /*
            var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            var dummyComponent = Substitute.For<IComponent>();

            entity.AddComponent(dummyComponent);

            mockEventSystem.Received().Publish(Arg.Is<ComponentAddedEvent>(x => x.Entity == entity && x.Component == dummyComponent));
            */
        }

        [Test]
        public void should_correctly_remove_component()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();

            /*
            var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            var dummyComponent = Substitute.For<IComponent>();
            entity.AddComponent(dummyComponent);

            entity.RemoveComponent(dummyComponent);

            Assert.That(entity.Components, Is.Empty);
            */
        }

        [Test]
        public void should_correctly_raise_event_when_removing_component()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();
            //var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            //var dummyComponent = Substitute.For<IComponent>();
            //entity.AddComponent(dummyComponent);
            //entity.RemoveComponent(dummyComponent);

            //mockEventSystem.Received().Publish(Arg.Is<ComponentRemovedEvent>(x => x.Entity == entity && x.Component == dummyComponent));
        }

        [Test]
        public void should_return_true_when_entity_has_component()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();
            //var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            //var dummyComponent = new TestComponentOne();
            //entity.AddComponent(dummyComponent);

            //Assert.That(entity.HasComponent<TestComponentOne>());
        }

        [Test]
        public void should_return_true_when_entity_matches_all_components()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();
            //var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            //entity.AddComponent(new TestComponentOne());
            //entity.AddComponent(new TestComponentTwo());

            //Assert.That(entity.HasComponents(typeof(TestComponentOne), typeof(TestComponentTwo)));
        }

        [Test]
        public void should_return_false_when_entity_does_not_match_all_components()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            //var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();
            //var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            //entity.AddComponent(new TestComponentOne());

            //Assert.IsFalse(entity.HasComponents(typeof(TestComponentOne), typeof(TestComponentTwo)));
        }

        [Test]
        public void should_return_false_when_entity_is_missing_component()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();
            //var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);

            //Assert.IsFalse(entity.HasComponent<TestComponentOne>());
        }

        [Test]
        public void should_throw_error_when_adding_component_that_already_exists()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();
            //var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            //var dummyComponent = Substitute.For<IComponent>();
            //entity.AddComponent(dummyComponent);

            //Assert.Throws<ArgumentException>(() => entity.AddComponent(dummyComponent));
        }

        [Test]
        public void should_not_throw_error_when_removing_non_existent_component_with_generic()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();
            //var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            //entity.RemoveComponent<TestComponentOne>();
        }

        [Test]
        public void should_not_throw_error_when_removing_non_existent_component_with_instance()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();
            //var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            //var dummyComponent = new TestComponentOne();

            //entity.RemoveComponent(dummyComponent);
        }

        [Test]
        public void should_remove_all_components_when_disposing()
        {
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockPool = Substitute.For<IPool>();
            var mockEntityIndexPool = Substitute.For<IEntityIndexPool>();
            //var entity = new Entity(mockEntityIndexPool.GetId(), mockPool, mockEventSystem);
            //var testComponentOne = entity.AddComponent<TestComponentOne>();
            //var testComponentTwo = entity.AddComponent<TestComponentTwo>();
            //var testComponentThree = entity.AddComponent<TestComponentThree>();

            //entity.Dispose();

            //mockEventSystem.Received().Publish(Arg.Is<ComponentRemovedEvent>(x => x.Entity == entity && x.Component == testComponentOne));
            //mockEventSystem.Received().Publish(Arg.Is<ComponentRemovedEvent>(x => x.Entity == entity && x.Component == testComponentTwo));
            //mockEventSystem.Received().Publish(Arg.Is<ComponentRemovedEvent>(x => x.Entity == entity && x.Component == testComponentThree));

            //Assert.That(entity.Components, Is.Empty);
        }
    }
}
