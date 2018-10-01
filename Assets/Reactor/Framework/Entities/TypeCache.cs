using System;

namespace Reactor.Entities
{
    public static class TypeCache<T>
    {
        public static readonly Type Type;

        public static readonly int TypeId;

        static TypeCache()
        {
            Type = typeof(T);
            if (Type == null)
                throw new Exception("Incorrect type initialization!");
            TypeId = TypeHelper.Initialize(Type);
        }
    }
}