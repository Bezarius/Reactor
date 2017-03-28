using Assets.Reactor.Examples.CustomGameObjectHandling.Components;
using Assets.Reactor.Examples.CustomGameObjectHandling.Systems;
using Reactor.Unity;
using Zenject;

namespace Assets.Reactor.Examples.CustomGameObjectHandling
{
    public class Application : ReactorApplication
    {
        [Inject]
        public CustomViewSetupSystem CustomViewSetupSystem { get; private set; }

        [Inject]
        public PlayerControlSystem PlayerControlSystem { get; private set; }

        [Inject]
        public CameraFollowSystem CameraFollowSystem { get; private set; }

        protected override void ApplicationStarted()
        {
            SystemExecutor.AddSystem(CustomViewSetupSystem);
            SystemExecutor.AddSystem(PlayerControlSystem);
            SystemExecutor.AddSystem(CameraFollowSystem);

            var defaultPool = PoolManager.GetPool();
            var viewEntity = defaultPool.CreateEntity();
            viewEntity.AddComponent(new CustomViewComponent());
            viewEntity.AddComponent(new PlayerControlledComponent());
            viewEntity.AddComponent(new CameraFollowsComponent());
        }
    }
}