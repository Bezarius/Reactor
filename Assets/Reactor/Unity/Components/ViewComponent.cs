using System;
using Assets.Reactor.Unity.ViewPooling;
using Reactor.Components;
using UnityEngine;

namespace Reactor.Unity.Components
{
    [Serializable]
    public class ViewComponent : EntityComponent<ViewComponent>
    {
        public GameObject GameObject;
        internal IViewPool ViewPool { get; set; }
    }
}