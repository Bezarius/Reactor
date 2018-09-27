using Reactor.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.SimpleMovement.Components
{
    public class CameraFollowsComponent : EntityComponent<CameraFollowsComponent>
    {
        public Camera Camera { get; set; }         
    }
}