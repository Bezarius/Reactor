using System;
using Reactor.Groups;
using Reactor.Systems;

namespace Reactor.Extensions
{
    public static class ISystemExtensions
    {
        public static IGroup GroupFor(this ISystem system, params Type[] componentTypes)
        {
            return new Group(componentTypes);
        }

        public static bool IsSystemReactive(this ISystem system)
        {
            return system is IEntityReactionSystem || system is IGroupReactionSystem || system is IInteractReactionSystem;
        }
    }
}