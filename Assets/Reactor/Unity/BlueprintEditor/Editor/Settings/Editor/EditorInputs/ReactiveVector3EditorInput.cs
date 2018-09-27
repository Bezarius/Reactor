using System.Reflection;
using Reactor.Components;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class ReactiveVector3EditorInput : BaseEditorInput<Vector3ReactiveProperty>
    {
        protected override Vector3ReactiveProperty Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var observable = GetComponentValue(component, fieldInfo);
            if (observable != null)
                observable.Value = EditorGUILayout.Vector3Field(fieldInfo.Name, observable.Value);
            return null;
        }
    }
}