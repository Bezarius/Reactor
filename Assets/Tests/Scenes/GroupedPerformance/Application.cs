﻿using Assets.Tests.Scenes.GroupedPerformance.Components;
using Assets.Tests.Scenes.GroupedPerformance.Systems;
using Assets.Tests.Scenes.GroupedPerformance.ViewResolvers;
using Reactor.Unity;
using Reactor.Unity.Components;

namespace Assets.Tests.Scenes.GroupedPerformance
{
    public class Application : ReactorApplication
    {
        private readonly int _cubeCount = 5000;

        protected override void ApplicationStarting()
        {
            RegisterBoundSystem<CubeViewResolver>();

            // Enable one of the below to see impact
            //RegisterBoundSystem<GroupRotationSystem>();
            //RegisterBoundSystem<EntityRotationSystem>();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();

            for (var i = 0; i < _cubeCount; i++)
            {
                var viewEntity = defaultPool.CreateEntity();
                viewEntity.AddComponent(new ViewComponent());
                viewEntity.AddComponent(new RotationComponent { RotationSpeed = 10.0f });
            }
        }
    }
}