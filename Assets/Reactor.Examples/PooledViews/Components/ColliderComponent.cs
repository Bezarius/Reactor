using Reactor.Components;
using UnityEngine;

public class ColliderComponent : EntityComponent<ColliderComponent>
{
    public Rigidbody Rigidbody { get; set; }
    public Collider Collider { get; set; }
}