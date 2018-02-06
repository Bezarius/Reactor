using System;
using Reactor.Components;
using UnityEngine;

namespace Reactor.Unity.Components
{
    [Serializable]
    public class ViewComponent : EntityComponent<ViewComponent>
    {
        public bool DestroyWithView;
        public GameObject View;
    }
}