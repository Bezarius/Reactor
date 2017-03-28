using Reactor.Unity.Components;
using UnityEngine;

namespace Assets.Reactor.Unity.Extensions
{
    public static class ViewComponentExtensions
    {
        public static void DestroyView(this ViewComponent viewComponent, float delay = 0.0f)
        {
            Object.Destroy(viewComponent.View, delay);
        }
    }
}