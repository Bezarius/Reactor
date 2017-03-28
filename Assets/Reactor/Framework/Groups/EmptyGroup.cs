using System;
using System.Collections.Generic;
using Reactor.Entities;

namespace Reactor.Groups
{
    public class EmptyGroup : IGroup
    {
        public IEnumerable<Type> TargettedComponents { get { return new Type[0]; } }
        public Predicate<IEntity> TargettedEntities { get { return null; } }
    }
}