using System;
using System.Reflection;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class EnumEditorInput : BaseEditorInput<Enum>
    {
        public override bool HandlesType(Type type)
        {
            return type.IsEnum;
        }

        protected override Enum Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            return EditorGUILayout.EnumPopup(fieldInfo.Name, GetComponentValue(component, fieldInfo));
        }
    }
}
