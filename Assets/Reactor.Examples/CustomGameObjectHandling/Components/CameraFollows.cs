using Reactor.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.CustomGameObjectHandling.Components
{
    public class CameraFollowsComponent : IComponent
    {
        public Camera Camera { get; set; }         
    }
}