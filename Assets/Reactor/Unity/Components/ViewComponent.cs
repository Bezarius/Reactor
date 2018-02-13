using System;
using Reactor.Components;
using UnityEngine;

namespace Reactor.Unity.Components
{
    [Serializable]
    public class ViewComponent : EntityComponent<ViewComponent>
    {
        public bool DestroyWithView;
        public GameObject GameObject;
        public Transform Transform;

        // initial params for transform
        public TransformSettings TransformSettings;
    }

    public class TransformSettings
    {
        public Vector3 Position = Vector3.zero;
        public Quaternion Rotation = Quaternion.identity;
        public Vector3 Scale = Vector3.one;
    }
}