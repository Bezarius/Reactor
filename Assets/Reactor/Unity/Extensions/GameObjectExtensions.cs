using System;
using Reactor.Entities;
using Reactor.Unity.Components;
using Reactor.Unity.MonoBehaviours;
using UnityEngine;

namespace Assets.Reactor.Unity.Extensions
{
    public static class GameObjectExtensions
    {
        public static void LinkEntity(this GameObject gameObject, IEntity entity)
        {
            if (gameObject.GetComponent<EntityView>())
            {
                throw new Exception("GameObject already has an EntityView monobehaviour applied");
            }

            if (!entity.HasComponent<ViewComponent>())
            {
                entity.AddComponent(new ViewComponent
                {
                    GameObject = gameObject
                });
            }
            else
            {
                var viewComponent = entity.GetComponent<ViewComponent>();
                viewComponent.GameObject = gameObject;
            }


            // conflict with ViewHandler
            //var entityViewMb = gameObject.AddComponent<EntityView>();
            //entityViewMb.Entity = entity;
        }
    }
}