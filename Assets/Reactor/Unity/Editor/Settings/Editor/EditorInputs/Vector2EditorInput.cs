using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class Vector2EditorInput : BaseEditorInput<Vector2>
    {
        protected override Vector2 Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.Vector2Field(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}