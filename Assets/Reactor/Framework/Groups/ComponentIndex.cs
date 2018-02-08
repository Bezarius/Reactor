using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Entities;

namespace Reactor.Groups
{
    public class ComponentIndex
    {
        private int[] _componentIdx;

        private readonly List<Type> _types;

        public ComponentIndex(List<Type> targetTypes)
        {
            _types = targetTypes;
        }

        // todo: check posibility for fast rebuild if new type in array(_componentIdx) range
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
                _componentIdx = Enumerable.Repeat(-1, ids.Max() + 1).ToArray();

                for (int j = 0; j < ids.Length; j++)
                {
                    _componentIdx[ids[j]] = j;
                }
            }
            else
            {
                _componentIdx = new int[0];
            }
        }

        public bool HasIndex(int index)
        {
            return index < _componentIdx.Length && _componentIdx[index] > -1;
        }

        public int GetTypeIndex(int typeId)
        {
            if (typeId < _componentIdx.Length)
                return _componentIdx[typeId];
            return -1;
        }

        public void AddToIndex(Type type)
        {
            _types.Add(type);
        }
    }
}