using System;
using System.Reflection;
using Reactor.Components;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public interface IEditorInput
    {
        bool HandlesType(Type type);
        void DrawComponentElementUI(GameObject gameObject, IComponent component, FieldInfo fieldInfo);
    }
}