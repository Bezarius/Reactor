using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class StringEditorInput : BaseEditorInput<string>
    {
        protected override string Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.TextField(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}