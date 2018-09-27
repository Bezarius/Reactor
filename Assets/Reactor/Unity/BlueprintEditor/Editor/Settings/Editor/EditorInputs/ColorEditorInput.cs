using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class ColorEditorInput : BaseEditorInput<Color>
    {
        protected override Color Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.ColorField(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}