using UniRx;
using UnityEditor;

namespace Reactor.Unity.Helpers.EditorInputs
{
    public class ReactiveRectEditorInput : SimpleEditorInput<RectReactiveProperty>
    {
        protected override RectReactiveProperty CreateTypeUI(string label, RectReactiveProperty value)
        {
            value.Value = EditorGUILayout.RectField(label, value.Value);
            return null;
        }
    }
}