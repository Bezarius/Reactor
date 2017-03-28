using System;
using System.Collections.Generic;
using Assets.Reactor.Examples.UsingBlueprints.Components;
using Reactor.Entities;
using Reactor.Groups;

namespace Assets.Reactor.Examples.UsingBlueprints.Groups
{
    public class PlayerGroup : IGroup
    {
        private readonly IEnumerable<Type> _components = new[]
        {
            typeof (HasName), typeof (WithHealthComponent)
        };

        public IEnumerable<Type> TargettedComponents { get { return _components; } }
        public Predicate<IEntity> TargettedEntities { get { return null; } }
    }
}