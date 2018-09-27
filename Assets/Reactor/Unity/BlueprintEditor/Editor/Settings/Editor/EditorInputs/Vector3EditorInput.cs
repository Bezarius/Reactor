using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class Vector3EditorInput : BaseEditorInput<Vector3>
    {
        protected override Vector3 Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.Vector3Field(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}