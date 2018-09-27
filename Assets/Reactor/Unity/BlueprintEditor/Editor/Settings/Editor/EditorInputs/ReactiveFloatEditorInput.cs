using System.Reflection;
using Reactor.Components;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class ReactiveFloatEditorInput : BaseEditorInput<FloatReactiveProperty>
    {
        protected override FloatReactiveProperty Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var observable = GetComponentValue(component, fieldInfo);
            if (observable != null)
                observable.Value = EditorGUILayout.FloatField(fieldInfo.Name, observable.Value);
            return null;
        }
    }
}