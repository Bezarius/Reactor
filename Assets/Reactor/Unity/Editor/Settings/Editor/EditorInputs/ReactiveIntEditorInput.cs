using System.Reflection;
using Reactor.Components;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class ReactiveIntEditorInput : BaseEditorInput<IntReactiveProperty>
    {
        protected override IntReactiveProperty Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var observable = GetComponentValue(component, fieldInfo);
            if (observable != null)
                observable.Value = EditorGUILayout.IntField(fieldInfo.Name, observable.Value);
            return null;
        }
    }

    public class ReactiveInt2EditorInput : BaseEditorInput<ReactiveProperty<int>>
    {
        protected override ReactiveProperty<int> Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var observable = GetComponentValue(component, fieldInfo);
            if (observable != null)
                observable.Value = EditorGUILayout.IntField(fieldInfo.Name, observable.Value);
            return null;
        }
    }
}