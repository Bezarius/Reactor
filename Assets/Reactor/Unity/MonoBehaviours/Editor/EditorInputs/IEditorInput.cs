using System;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public interface IEditorInput
    {
        bool HandlesType(Type type);
        object CreateUI(string label, object value);
    }
}