using Reactor.Components;
using UnityEngine;

namespace Reactor.Unity.Components
{
    public class ViewComponent : IComponent
    {
        public bool DestroyWithView { get; set; }
        public GameObject View { get; set; }
    }
}