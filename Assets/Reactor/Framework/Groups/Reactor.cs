using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Reactor.Components;
using Reactor.Entities;
using Reactor.Extensions;
using Reactor.Systems;
using Reactor.Systems.Executor;

namespace Reactor.Groups
{

    public interface ISystemContainer
    {
        ISetupSystem[] SetupSystems { get; }
        IEntityReactionSystem[] EntityReactionSystems { get; }
        IGroupReactionSystem[] GroupReactionSystems { get; }
        IInteractReactionSystem[] InteractReactionSystems { get; }
        ITeardownSystem[] TeardownSystems { get; }
        IGroupAccessor[] GroupAccessors { get; }

        bool HasGroupOrSystems { get; }

        void AddGroupAccessor(IGroupAccessor groupAccessor);
    }

    public sealed class SystemReactor : ISystemReactor
    {
        private readonly ISystemExecutor _systemExecutor;
        public readonly HashSet<Type> TargetTypesSet;
        public List<Type> TargetTypesList;

        public ISetupSystem[] SetupSystems { get; private set; }
        public IEntityReactionSystem[] EntityReactionSystems { get; private set; }
        public IGroupReactionSystem[] GroupReactionSystems { get; private set; }
        public IInteractReactionSystem[] InteractReactionSystems { get; private set; }
        public ITeardownSystem[] TeardownSystems { get; private set; }
        public IGroupAccessor[] GroupAccessors { get; private set; }

        public bool HasGroupOrSystems { get; private set; }


        internal readonly ComponentIndex _componentIndex;
        private readonly ConnectionIndex _inConnectionIndex;
        private readonly ConnectionIndex _outConnectionIndex;

        // todo: вынести создание в фабрику
        public SystemReactor(ISystemExecutor systemExecutor, HashSet<Type> targetTypes)
        {
            _systemExecutor = systemExecutor;
            TargetTypesSet = targetTypes;
            TargetTypesList = targetTypes.ToList();

            _componentIndex = new ComponentIndex(targetTypes.ToList());
            _componentIndex.Build();

            _inConnectionIndex = new ConnectionIndex(targetTypes.ToList());
            _outConnectionIndex = new ConnectionIndex(targetTypes.ToList());

            var systems = _systemExecutor.Systems
                .Where(x => x.TargetGroup.TargettedComponents.Any() && x.TargetGroup.TargettedComponents.All(targetTypes.Contains))
                .ToList();

            GroupAccessors = _systemExecutor.PoolManager.PoolAccessors
                .Where(x => new HashSet<Type>(x.AccessorToken.ComponentTypes).IsSubsetOf(targetTypes))
                .ToArray();

            SetupSystems = systems.OfType<ISetupSystem>().OrderByPriority().ToArray();
            EntityReactionSystems = systems.OfType<IEntityReactionSystem>().OrderByPriority().ToArray();
            GroupReactionSystems = systems.OfType<IGroupReactionSystem>().OrderByPriority().ToArray();
            InteractReactionSystems = systems.OfType<IInteractReactionSystem>().OrderByPriority().ToArray();
            TeardownSystems = systems.OfType<ITeardownSystem>().OrderByPriority().ToArray();

            HasGroupOrSystems = systems.Count > 0 || GroupAccessors.Length > 0;
        }

        public void AddEntityToReactor(IEntity entity)
        {
            _systemExecutor.AddSystemsToEntity(entity, this);
        }

        public void AddComponent(IEntity entity, IComponent component)
        {
            ReactorConnection connection;
            var typeId = component.TypeId;
            if (!_outConnectionIndex.TryGetValue(typeId, out connection))
            {
                var typeList = new List<Type>(TargetTypesSet) { component.Type };
                SystemReactor nextReactor = _systemExecutor.GetOrCreateConcreteSystemReactor(typeList);
                connection = new ReactorConnection(component.Type, nextReactor, this);
                _outConnectionIndex.Add(component, connection);
                nextReactor._inConnectionIndex.Add(component, connection);
            }
            entity.Reactor = connection.UpReactor;
            _systemExecutor.AddSystemsToEntity(entity, connection);
        }

        public void RemoveComponent(IEntity entity, IComponent component)
        {
            ReactorConnection connection;
            var typeId = component.TypeId;
            if (!_inConnectionIndex.TryGetValue(typeId, out connection))
            {
                var typeList = new List<Type>(TargetTypesSet);
                typeList.Remove(component.Type);
                SystemReactor prevReactor = _systemExecutor.GetOrCreateConcreteSystemReactor(typeList);
                connection = new ReactorConnection(component.Type, this, prevReactor);
                _inConnectionIndex.Add(component, connection);
                prevReactor._outConnectionIndex.Add(component, connection);
            }
            _systemExecutor.RemoveSystemsFromEntity(entity, connection);
            entity.Reactor = connection.DownReactor;
        }

        public void AddGroupAccessor(IGroupAccessor groupAccessor)
        {
            // add to this
            var groupAccessors = GroupAccessors;
            var gaLen = GroupAccessors.Length;
            Array.Resize(ref groupAccessors, gaLen + 1);
            GroupAccessors = groupAccessors;
            GroupAccessors[gaLen] = groupAccessor;

            // check actual connections
            foreach (var type in groupAccessor.AccessorToken.ComponentTypes)
            {
                ReactorConnection connection;
                var typeId = TypeHelper.GetTypeId(type);
                if (_outConnectionIndex.TryGetValue(typeId, out connection) && connection.UpReactor == this)
                {
                    connection.AddGroupAccessor(groupAccessor);
                }
            }
        }

        public int GetComponentIdx(int componentId)
        {
            return _componentIndex.GetTypeIndex(componentId);
        }

        public bool HasComponentIndex(int componentId)
        {
            return _componentIndex.HasIndex(componentId);
        }

        public int GetFutureComponentIdx(IComponent component)
        {
            var typeId = component.TypeId;
            var id = _componentIndex.GetTypeIndex(component.TypeId);
            if (id == -1)
            {
                // компонент не содержится в текущем реакторе. 
                // Данная ветка необходима для определение "будущего" идентификатора в следующем реакторе
                ReactorConnection connection;
                if (!_outConnectionIndex.TryGetValue(typeId, out connection))
                {
                    var typeList = new List<Type>(TargetTypesSet) { component.Type };
                    SystemReactor nextReactor = _systemExecutor.GetOrCreateConcreteSystemReactor(typeList);
                    connection = new ReactorConnection(component.Type, nextReactor, this);
                    _outConnectionIndex.Add(component, connection);
                    nextReactor._inConnectionIndex.Add(component, connection);
                }
                id = connection.UpReactor.GetComponentIdx(typeId);
            }
            return id;
        }
    }
}