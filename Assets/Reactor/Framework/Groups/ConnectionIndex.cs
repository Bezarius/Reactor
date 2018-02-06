using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Components;

namespace Reactor.Groups
{
    public class ConnectionIndex
    {
        private readonly ComponentIndex _index;
        private ReactorConnection[] _connections;

        public ConnectionIndex(IEnumerable<Type> targetTypes)
        {
            _index = new ComponentIndex(targetTypes.ToList());
            _index.Build();
            _connections = new ReactorConnection[0];
        }

        public void Add(IComponent component, ReactorConnection connection)
        {
            if (!_index.HasIndex(component.TypeId))
            {
                _index.AddToIndex(component.Type);
                _index.Build();
            }

            var idx = _index.GetTypeIndex(component.TypeId);

            if (idx >= _connections.Length)
                Array.Resize(ref _connections, idx + 1);

            if (_connections[idx] == null)
                _connections[idx] = connection;
            
        }

        public bool TryGetValue(int typeId, out ReactorConnection connection)
        {
            var idx = _index.GetTypeIndex(typeId);

            if (-1 < idx && idx < _connections.Length)
            {
                connection = _connections[idx];
                return connection != null;
            }
            connection = null;
            return false;
        }
    }
}