using System;
using System.Collections.Generic;
using Reactor.Components;

namespace Reactor.Groups
{
    public class GroupBuilder
    {
        private List<Type> _components;

        public GroupBuilder()
        {
            _components = new List<Type>();
        }

        public GroupBuilder Create()
        {
            _components = new List<Type>();
            return this;
        }

        public GroupBuilder WithComponent<T>() where T : class, IComponent
        {
            _components.Add(typeof(T));
            return this;
        }


        public IGroup Build()
        {
            return new Group(_components.ToArray());
        }
    }
}