using UnityEditor;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class IntEditorInput : SimpleEditorInput<int>
    {
        protected override int CreateTypeUI(string label, int value)
        { return EditorGUILayout.IntField(label, value); }
    }
}