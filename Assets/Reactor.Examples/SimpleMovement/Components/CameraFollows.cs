using Reactor.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.SimpleMovement.Components
{
    public class CameraFollowsComponent : IComponent
    {
        public Camera Camera { get; set; }         
    }
}