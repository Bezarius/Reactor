using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public static class DefaultEditorInputRegistry
    {
        private static readonly EditorInputRegistry _editorInputRegistry;

        static DefaultEditorInputRegistry()
        {
            _editorInputRegistry = new EditorInputRegistry(new List<IEditorInput>
            {
                new IntEditorInput(),
                new FloatEditorInput(),
                new StringEditorInput(),
                new BoolEditorInput(),
                new Vector2EditorInput(),
                new Vector3EditorInput(),
                new ColorEditorInput(),
                new BoundsEditorInput(),
                new RectEditorInput(),
                new EnumEditorInput(),
                new ReactiveIntEditorInput(),
                new ReactiveInt2EditorInput(),
                new ReactiveFloatEditorInput(),
                new ReactiveStringEditorInput(),
                new ReactiveBoolEditorInput(),
                new ReactiveVector2EditorInput(),
                new ReactiveVector3EditorInput(),
                new ReactiveColorEditorInput(),
                new ReactiveBoundsEditorInput(),
                new ReactiveRectEditorInput(),
                new GameObjectEditorInput(),
                new BehaviourEditorInput()
            });
        }

        public static IEditorInput GetHandlerFor(Type type)
        {
            return _editorInputRegistry.GetHandlerFor(type);
        }
    }
}