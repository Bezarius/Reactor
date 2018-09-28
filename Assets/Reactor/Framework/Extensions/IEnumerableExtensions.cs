using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Attributes;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;

namespace Reactor.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEachRun<T>(this IEnumerable<T> enumerable, Action<T> method)
        {
            foreach (var element in enumerable)
            { method(element); }
        }

        public static IEnumerable<Tout> ForEachRun<Tin, Tout>(this IEnumerable<Tin> enumerable, Func<Tin, Tout> method)
        {
            return enumerable.Select(method);
        }

        public static IEnumerable<IEntity> MatchingGroup(this IEnumerable<IEntity> entities, IGroup group)
        {
            var componentTypes = group.TargettedComponents.ToArray();
            return entities.Where(x => x.HasComponents(componentTypes));
        }

        public static IEnumerable<ISystem> GetApplicableSystems(this IEnumerable<ISystem> systems, IEntity entity)
        {
            return systems.Where(x => entity.MatchesGroup(x.TargetGroup));
        }


        public static IEnumerable<T> OrderByPriority<T>(this IEnumerable<T> systems)
            where T : ISystem
        {
            return systems.OrderByDescending(x =>
            {
                var priorityAttributes = x.GetType().GetCustomAttributes(typeof (PriorityAttribute), true);
                if (priorityAttributes.Length <= 0) return 0;
                var priorityAttribute = priorityAttributes.FirstOrDefault() as PriorityAttribute;
                return priorityAttribute.Priority;
            });
        } 
    }
}