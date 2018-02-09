using System;
using Reactor.Entities;
using Reactor.Unity.Components;
using UnityEngine;

namespace Assets.Reactor.Unity.Extensions
{
    public static class IEntityExtensions
    {
        public static T GetComponent<T>(this IEntity entity) where T : MonoBehaviour
        {
            if (!entity.HasComponent<ViewComponent>())
            { return null; }

            var viewComponent = entity.GetComponent<ViewComponent>();

            if (!viewComponent.GameObject)
            { return null; }

            return viewComponent.GameObject.GetComponent<T>();
        }

        public static T AddComponent<T>(this IEntity entity) where T : MonoBehaviour
        {
            if (!entity.HasComponent<ViewComponent>())
            { throw new Exception("Entity has no ViewComponent, ensure a valid ViewComponent is applied with an active View"); }

            var viewComponent = entity.GetComponent<ViewComponent>();

            if (!viewComponent.GameObject)
            { throw new Exception("Entity's ViewComponent has no assigned View, GameObject has been applied to the View"); }

            return viewComponent.GameObject.AddComponent<T>();
        }
    }
}