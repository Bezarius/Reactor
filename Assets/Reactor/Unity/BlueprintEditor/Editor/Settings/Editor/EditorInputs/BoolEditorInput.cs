using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class BoolEditorInput : BaseEditorInput<bool>
    {
        protected override bool Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.Toggle(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}