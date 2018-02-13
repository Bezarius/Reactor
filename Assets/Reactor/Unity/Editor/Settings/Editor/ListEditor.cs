using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class ListEditor : ModuleEditor
{
    [SerializeField]
    private Rect _listRect = new Rect(0, 0, 200, 1000);

    internal sealed override void OnGUI()
    {
        _listRect.height = Position.height - _listRect.y;
        GUILayout.BeginArea(_listRect, "", "box");
        OnListGUI();
        GUILayout.EndArea();
        var rect = new Rect(_listRect.width, _listRect.y, Position.width - _listRect.width, Position.height - _listRect.y);
        GUILayout.BeginArea(rect, "", "hostview");
        OnValueGUI();
        GUILayout.EndArea();
        ResizeMenuHorizontal();
    }

    protected abstract void OnListGUI();

    protected abstract void OnValueGUI();

    private void ResizeMenuHorizontal()
    {
        var rect = new Rect(_listRect.width - 5, _listRect.y, 10, _listRect.height);
        EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
        var controlId = GUIUtility.GetControlID(FocusType.Passive);
        var ev = Event.current;
        switch (ev.rawType)
        {
            case EventType.MouseDown:
                if (rect.Contains(ev.mousePosition))
                {
                    GUIUtility.hotControl = controlId;
                    ev.Use();
                }
                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlId)
                {
                    GUIUtility.hotControl = 0;
                    ev.Use();
                }
                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlId)
                {
                    _listRect.width = ev.mousePosition.x;
                    _listRect.width = Mathf.Clamp(_listRect.width, 200, 400);
                    ev.Use();
                }
                break;
        }
    }
}
