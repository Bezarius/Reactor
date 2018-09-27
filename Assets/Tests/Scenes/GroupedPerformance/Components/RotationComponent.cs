using Reactor.Components;

namespace Assets.Tests.Scenes.GroupedPerformance.Components
{
    public class RotationComponent : EntityComponent<RotationComponent>
    {
        public float RotationSpeed { get; set; }
    }
}