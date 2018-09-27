using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class BoundsEditorInput : BaseEditorInput<Bounds>
    {
        protected override Bounds Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.BoundsField(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}