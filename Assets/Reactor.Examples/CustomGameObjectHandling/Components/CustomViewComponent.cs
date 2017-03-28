using Reactor.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.CustomGameObjectHandling.Components
{
    public class CustomViewComponent : IComponent
    {
        public GameObject CustomView { get; set; }
    }
}