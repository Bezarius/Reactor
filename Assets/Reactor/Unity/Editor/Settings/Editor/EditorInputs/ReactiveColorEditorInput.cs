using System.Reflection;
using Reactor.Components;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class ReactiveColorEditorInput : BaseEditorInput<ColorReactiveProperty>
    {
        protected override ColorReactiveProperty Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var observable = GetComponentValue(component, fieldInfo);
            if (observable != null)
                observable.Value = EditorGUILayout.ColorField(fieldInfo.Name, observable.Value);
            return null;
        }
    }
}