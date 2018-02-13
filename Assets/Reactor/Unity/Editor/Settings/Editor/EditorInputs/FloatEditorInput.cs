using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class FloatEditorInput : BaseEditorInput<float>
    {
        protected override float Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.FloatField(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}