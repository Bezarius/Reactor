using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Reactor.Entities;

namespace Reactor.Groups
{
    public sealed class ComponentIndex
    {
        private int[] _componentIdx;

        private readonly List<Type> _types;

        public ComponentIndex(List<Type> targetTypes)
        {
            _types = targetTypes;
        }

        public void Build()
        {
            if (_types.Count > 0)
            {
                var ids = new int[_types.Count];
                var i = 0;

                foreach (var targetType in _types)
                {
                    ids[i] = TypeHelper.GetTypeId(targetType);
                    i++;
                }

                // fill array -1
                _componentIdx = Enumerable.Repeat(-1, TypeHelper.Counter + 1).ToArray();

                for (int j = 0; j < ids.Length; j++)
                {
                    _componentIdx[ids[j]] = j;
                }
            }
            else
            {
                _componentIdx = Enumerable.Repeat(-1, TypeHelper.Counter + 1).ToArray();
            }
        }

        public bool HasIndex(int index)
        {
            return _componentIdx[index] > -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetTypeIndex(int typeId)
        {
            return _componentIdx[typeId];
        }

        public void AddToIndex(Type type)
        {
            _types.Add(type);
        }
    }
}