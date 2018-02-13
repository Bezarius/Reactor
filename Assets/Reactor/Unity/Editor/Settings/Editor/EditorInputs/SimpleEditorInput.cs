using System;
using System.Reflection;
using Reactor.Components;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public abstract class BaseEditorInput<T> : IEditorInput
    {
        public virtual bool HandlesType(Type type)
        {
            return type == typeof(T);
        }

        public void DrawComponentElementUI(GameObject gameObject, IComponent component, FieldInfo fieldInfo)
        {
            var value = Draw(gameObject, component, fieldInfo);
            if (value != null)
                fieldInfo.SetValue(component, value);
        }

        protected T GetComponentValue(IComponent component, FieldInfo fieldInfo)
        {
            return (T)fieldInfo.GetValue(component);
        }

        protected abstract T Draw(GameObject gameObject, IComponent component, FieldInfo fieldInfo);
    }
}
