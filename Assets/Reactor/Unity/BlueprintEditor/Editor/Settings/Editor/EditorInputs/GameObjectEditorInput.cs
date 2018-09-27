using System;
using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class GameObjectEditorInput : BaseEditorInput<GameObject>
    {
        protected override GameObject Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return (GameObject)EditorGUILayout.ObjectField(fieldInfo.Name, GetComponentValue(component, fieldInfo), typeof(GameObject), true);
        }
    }

    public class BehaviourEditorInput : BaseEditorInput<Behaviour>
    {
        public override bool HandlesType(Type type)
        {
            return type.IsSubclassOf(typeof(Behaviour));
        }

        protected override Behaviour Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return (Behaviour)EditorGUILayout.ObjectField(fieldInfo.Name, GetComponentValue(component, fieldInfo), typeof(Behaviour), true);
        }
    }
}
