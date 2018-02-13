using UnityEngine;

public static class GUIStyleHelper
{
    private static GUIStyle _menu;

    public static GUIStyle Menu
    {
        get
        {
            return _menu ?? (_menu =
                       new GUIStyle((GUIStyle)"ProfilerLeftPane") { padding = new RectOffset(0, 2, 0, 0) });
        }
    }

    private static GUIStyle _plus;

    public static GUIStyle Plus
    {
        get { return _plus ?? (_plus = new GUIStyle("OL Plus")); }
    }

    private static GUIStyle _minus;

    public static GUIStyle Minus
    {
        get { return _minus ?? (_minus = new GUIStyle("OL Minus") { margin = new RectOffset(0, 0, 5, 0) }); }
    }

    private static GUIStyle _title;

    public static GUIStyle Title
    {
        get { return _title ?? (_title = new GUIStyle("label") { fontSize = 14 }); }
    }

    private static GUIStyle _line;

    public static GUIStyle Line
    {
        get
        {
            return _line ?? (_line = new GUIStyle("ShurikenLine")
            {
                fontSize = 14,
                normal = { textColor = ((GUIStyle)"label").normal.textColor },
                contentOffset = new Vector2(3, -2)
            });
        }
    }
}