using System.Reflection;
using Reactor.Components;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class ReactiveBoolEditorInput : BaseEditorInput<BoolReactiveProperty>
    {
        protected override BoolReactiveProperty Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var observable = GetComponentValue(component, fieldInfo);
            if (observable != null)
                observable.Value = EditorGUILayout.Toggle(fieldInfo.Name, observable.Value);
            return null;
        }
    }

    public class ReactiveBool2EditorInput : BaseEditorInput<ReactiveProperty<bool>>
    {
        protected override ReactiveProperty<bool> Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var observable = GetComponentValue(component, fieldInfo);
            if (observable != null)
                observable.Value = EditorGUILayout.Toggle(fieldInfo.Name, observable.Value);
            return null;
        }
    }
}