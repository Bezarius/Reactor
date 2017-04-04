using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Entities;
using Reactor.Extensions;
using Reactor.Systems;
using Reactor.Systems.Executor;
using Reactor.Components;

namespace Reactor.Groups
{

    public interface ISystemContainer
    {
        ISetupSystem[] SetupSystems { get; }
        IEntityReactionSystem[] EntityReactionSystems { get; }
        IGroupReactionSystem[] GroupReactionSystems { get; }
        IInteractReactionSystem[] InteractReactionSystems { get; }
        ITeardownSystem[] TeardownSystems { get; }
        IGroupAccessor[] GroupAccessors { get;  }

        void AddGroupAccessor(IGroupAccessor groupAccessor);
    }


    public class ReactorConnection : ISystemContainer
    {
        public SystemReactor UpReactor { get; private set; }
        public SystemReactor DownReactor { get; private set; }

        public ISetupSystem[] SetupSystems { get; private set; }
        public IEntityReactionSystem[] EntityReactionSystems { get; private set; }
        public IGroupReactionSystem[] GroupReactionSystems { get; private set; }
        public IInteractReactionSystem[] InteractReactionSystems { get; private set; }
        public ITeardownSystem[] TeardownSystems { get; private set; }
        public IGroupAccessor[] GroupAccessors { get; private set; }

        public ReactorConnection(SystemReactor upReactor, SystemReactor downReactor)
        {
            UpReactor = upReactor;
            DownReactor = downReactor;
            GroupAccessors = upReactor.GroupAccessors.Except(downReactor.GroupAccessors).ToArray();
            SetupSystems = upReactor.SetupSystems.Except(downReactor.SetupSystems).ToArray();
            EntityReactionSystems = upReactor.EntityReactionSystems.Except(downReactor.EntityReactionSystems).ToArray();
            GroupReactionSystems = upReactor.GroupReactionSystems.Except(downReactor.GroupReactionSystems).ToArray();
            InteractReactionSystems = upReactor.InteractReactionSystems.Except(downReactor.InteractReactionSystems).ToArray();
            TeardownSystems = upReactor.TeardownSystems.Except(downReactor.TeardownSystems).ToArray();
        }

        public void AddGroupAccessor(IGroupAccessor groupAccessor)
        {
            var groupAccessors = GroupAccessors;
            Array.Resize(ref groupAccessors, GroupAccessors.Length + 1);
            GroupAccessors[GroupAccessors.Length] = groupAccessor;
        }
    }

    public class SystemReactor : ISystemContainer
    {
        private readonly ISystemExecutor _systemExecutor;
        public readonly HashSet<Type> TargetTypes;

        public ISetupSystem[] SetupSystems { get; private set; }
        public IEntityReactionSystem[] EntityReactionSystems { get; private set; }
        public IGroupReactionSystem[] GroupReactionSystems { get; private set; }
        public IInteractReactionSystem[] InteractReactionSystems { get; private set; }
        public ITeardownSystem[] TeardownSystems { get; private set; }
        public IGroupAccessor[] GroupAccessors { get; private set; }

        private readonly Dictionary<Type, ReactorConnection> _connections = new Dictionary<Type, ReactorConnection>();

        // todo: вынести создание в фабрику
        public SystemReactor(ISystemExecutor systemExecutor, HashSet<Type> targetTypes)
        {
            _systemExecutor = systemExecutor;
            TargetTypes = targetTypes;

            var systems = _systemExecutor.Systems.Where(x => x.TargetGroup.TargettedComponents.All(targetTypes.Contains)).ToList();
            GroupAccessors = _systemExecutor.PoolManager.PoolAccessors.Where(x=>new HashSet<Type>(x.AccessorToken.ComponentTypes).IsSubsetOf(targetTypes)).ToArray();
            SetupSystems = systems.OfType<ISetupSystem>().OrderByPriority().ToArray();
            EntityReactionSystems = systems.OfType<IEntityReactionSystem>().OrderByPriority().ToArray();
            GroupReactionSystems = systems.OfType<IGroupReactionSystem>().OrderByPriority().ToArray();
            InteractReactionSystems = systems.OfType<IInteractReactionSystem>().OrderByPriority().ToArray();
            TeardownSystems = systems.OfType<ITeardownSystem>().OrderByPriority().ToArray();
        }

        public void AddReactorsConnection(Type componenType, ReactorConnection connection, SystemReactor reactor)
        {
            if (!_connections.ContainsKey(componenType))
            {
                _connections.Add(componenType, connection);
                reactor.AddReactorsConnection(componenType, connection, this);
            }
        }

        public void AddComponent(IEntity entity, Type componenType)
        {
            ReactorConnection connection;
            if (!_connections.TryGetValue(componenType, out connection))
            {
                var set = new HashSet<Type>(TargetTypes) {componenType};
                SystemReactor reactor = _systemExecutor.GetSystemReactor(set);
                connection = new ReactorConnection(reactor, this);
                this.AddReactorsConnection(componenType, connection, reactor);
            }
            //entity.Reactor = connection.UpReactor;
            _systemExecutor.AddSystemsToEntity(entity, connection);
        }

        public void RemoveComponent(IEntity entity, IComponent component)
        {
            ReactorConnection connection;
            var componentType = component.GetType();
            if (!_connections.TryGetValue(componentType, out connection))
            {
                var set = new HashSet<Type>(TargetTypes);
                SystemReactor reactor = _systemExecutor.GetSystemReactor(set);
                connection = new ReactorConnection(this, reactor);
                this.AddReactorsConnection(componentType, connection, reactor);
            }
            //entity.Reactor = connection.DownReactor;
            _systemExecutor.RemoveSystemsFromEntity(entity, connection);
        }

        public void AddGroupAccessor(IGroupAccessor groupAccessor)
        {
            // add to this
            var groupAccessors = GroupAccessors;
            Array.Resize(ref groupAccessors, GroupAccessors.Length + 1);
            GroupAccessors[GroupAccessors.Length] = groupAccessor;

            // check actual connections
            foreach (var type in groupAccessor.AccessorToken.ComponentTypes)
            {
                ReactorConnection connection;
                if (_connections.TryGetValue(type, out connection) && connection.UpReactor == this)
                {
                    connection.AddGroupAccessor(groupAccessor);
                }
            }
        }
    }
}