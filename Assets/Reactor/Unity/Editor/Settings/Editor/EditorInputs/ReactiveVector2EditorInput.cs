using System.Reflection;
using Reactor.Components;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class ReactiveVector2EditorInput : BaseEditorInput<Vector2ReactiveProperty>
    {
        protected override Vector2ReactiveProperty Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var observable = GetComponentValue(component, fieldInfo);
            if (observable != null)
                observable.Value = EditorGUILayout.Vector2Field(fieldInfo.Name, observable.Value);
            return null;
        }
    }
}