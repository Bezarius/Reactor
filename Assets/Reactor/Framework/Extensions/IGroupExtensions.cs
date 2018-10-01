using System;
using System.Collections.Generic;
using Reactor.Components;
using Reactor.Groups;

namespace Reactor.Extensions
{
    public static class IGroupExtensions
    {
        public static IGroup WithComponent<T>(this IGroup group) where T : class, IComponent
        {
            var componentTypes = new List<Type>(group.TargettedComponents) {typeof(T)};
            return new Group(componentTypes.ToArray());
        }
    }
}