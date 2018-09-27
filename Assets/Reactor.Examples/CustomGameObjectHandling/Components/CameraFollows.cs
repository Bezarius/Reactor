using Reactor.Components;
using UnityEngine;

namespace Assets.Reactor.Examples.CustomGameObjectHandling.Components
{
    public class CameraFollowsComponent : EntityComponent<CameraFollowsComponent>
    {
        public Camera Camera { get; set; }         
    }
}