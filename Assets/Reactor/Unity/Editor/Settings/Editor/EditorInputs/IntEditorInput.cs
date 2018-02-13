using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class IntEditorInput : BaseEditorInput<int>
    {
        protected override int Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.IntField(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}