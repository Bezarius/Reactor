using Reactor.Systems.Executor.Handlers;

namespace Reactor.Systems.Executor
{
    public interface ISystemHandlerManager
    {
        IEntityReactionSystemHandler EntityReactionSystemHandler { get; }
        IGroupReactionSystemHandler GroupReactionSystemHandler { get; }
        IInteractReactionSystemHandler InteractReactionSystemHandler { get; }
        IManualSystemHandler ManualSystemHandler { get; }
        ISetupSystemHandler SetupSystemHandler { get; }
    }
}