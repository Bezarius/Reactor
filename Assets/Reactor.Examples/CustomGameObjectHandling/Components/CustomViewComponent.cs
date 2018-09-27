using Reactor.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.CustomGameObjectHandling.Components
{
    public class CustomViewComponent : EntityComponent<CustomViewComponent>
    {
        public GameObject CustomView { get; set; }
    }
}