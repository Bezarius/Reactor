using System;
using System.Collections.Generic;
using Assets.Reactor.Examples.GroupFilters.Components;
using Reactor.Groups;

namespace Assets.Reactor.Examples.GroupFilters.Groups
{
    public class HasScoreGroup : IGroup
    {
        public IEnumerable<Type> TargettedComponents
        {
            get
            {
                return new[] { typeof (HasScoreComponent) };
            }
        }
    }
}