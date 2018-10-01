using System;
using System.Collections.Generic;

namespace Reactor.Entities
{
    public static class TypeHelper
    {
        public static int Counter { get; private set; }

        private static readonly Dictionary<Type, int> TypeDict = new Dictionary<Type, int>();

        public static int GetTypeId(Type type)
        {
            if (!TypeDict.TryGetValue(type, out var idx))
                idx = Initialize(type);
            return idx;
        }

        public static int Initialize(Type type)
        {
            Counter++;
#if DEBUG
            if (TypeDict.ContainsKey(type))
                throw new Exception($@"Type '{type}' is already initialized");
#endif
            TypeDict[type] = Counter;
            return Counter;
        }
    }
}