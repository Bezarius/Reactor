using System.Reflection;
using Reactor.Components;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class ReactiveBoundsEditorInput : BaseEditorInput<BoundsReactiveProperty>
    {
        protected override BoundsReactiveProperty Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var observable = GetComponentValue(component, fieldInfo);
            if (observable != null)
                observable.Value = EditorGUILayout.BoundsField(fieldInfo.Name, observable.Value);
            return null;
        }
    }
}