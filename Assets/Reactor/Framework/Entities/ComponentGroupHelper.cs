using System.Reflection;
using Reactor.Groups;

namespace Reactor.Entities
{
    public static class ComponentGroupHelper
    {
        public static void Initialize(IGroup @group)
        {
            var tt = typeof(TypeCache<>);

            foreach (var type in @group.TargettedComponents)
            {
                var args = new[] { type };
                var cache = tt.MakeGenericType(args);
                var field = cache.GetField("TypeId", BindingFlags.Static | BindingFlags.Public);
                var result = field.GetValue(null);
            }
        }
    }
}