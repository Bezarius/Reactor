using Reactor.Components;
using UnityEngine;

public class ColliderComponent : IComponent
{
    public Rigidbody Rigidbody { get; set; }
    public Collider Collider { get; set; }
}