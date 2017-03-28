using Assets.Reactor.Unity.Extensions;
using Reactor.Unity;
using UnityEngine;

namespace Assets.Reactor.Examples.GameObjectLinking
{
    public class Application : ReactorApplication
    {
        protected override void ApplicationStarting()
        {
            RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            var defaultPool = PoolManager.GetPool();
            var entity = defaultPool.CreateEntity();
            var existingGameObject = GameObject.Find("ExistingGameObject");
            existingGameObject.LinkEntity(entity);
        }
    }
}