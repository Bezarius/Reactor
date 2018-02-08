using System;
using System.Linq;
using Reactor.Extensions;
using Reactor.Systems;

namespace Reactor.Groups
{
    public class ReactorConnection : ISystemContainer
    {
        public Type ComponenType { get; private set; }

        public SystemReactor UpReactor { get; private set; }
        public SystemReactor DownReactor { get; private set; }

        public ISetupSystem[] SetupSystems { get; private set; }
        public IEntityReactionSystem[] EntityReactionSystems { get; private set; }
        public IGroupReactionSystem[] GroupReactionSystems { get; private set; }
        public IInteractReactionSystem[] InteractReactionSystems { get; private set; }
        public ITeardownSystem[] TeardownSystems { get; private set; }
        public IGroupAccessor[] GroupAccessors { get; private set; }

        public bool HasGroupOrSystems { get; private set; }

        public ReactorConnection(Type componentType, SystemReactor upReactor, SystemReactor downReactor)
        {
            ComponenType = componentType;
            UpReactor = upReactor;
            DownReactor = downReactor;
            GroupAccessors = upReactor.GroupAccessors.Except(downReactor.GroupAccessors).ToArray();
            SetupSystems = upReactor.SetupSystems.Except(downReactor.SetupSystems).OrderByPriority().ToArray();
            EntityReactionSystems = upReactor.EntityReactionSystems.Except(downReactor.EntityReactionSystems).OrderByPriority().ToArray();
            GroupReactionSystems = upReactor.GroupReactionSystems.Except(downReactor.GroupReactionSystems).OrderByPriority().ToArray();
            InteractReactionSystems = upReactor.InteractReactionSystems.Except(downReactor.InteractReactionSystems).OrderByPriority().ToArray();
            TeardownSystems = upReactor.TeardownSystems.Except(downReactor.TeardownSystems).OrderByPriority().ToArray();

            HasGroupOrSystems =
                GroupAccessors.Length > 0 ||
                SetupSystems.Length > 0 ||
                EntityReactionSystems.Length > 0 ||
                GroupReactionSystems.Length > 0 ||
                InteractReactionSystems.Length > 0 ||
                TeardownSystems.Length > 0;

        }

        public void AddGroupAccessor(IGroupAccessor groupAccessor)
        {
            var groupAccessors = GroupAccessors;
            Array.Resize(ref groupAccessors, GroupAccessors.Length + 1);
            GroupAccessors[GroupAccessors.Length] = groupAccessor;
        }
    }
}