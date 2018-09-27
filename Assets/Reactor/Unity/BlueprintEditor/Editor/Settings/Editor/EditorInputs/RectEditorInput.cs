using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class RectEditorInput : BaseEditorInput<Rect>
    {
        protected override Rect Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.RectField(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}