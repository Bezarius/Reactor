using UnityEngine;

public static class ToolbarSearchUI
{
    private static readonly GUIStyle TextFieldStyle;
    private static readonly GUIStyle CancelButtonStyle;
    private static readonly GUIStyle GroupStyle;

    static ToolbarSearchUI()
    {
        TextFieldStyle = GUI.skin.FindStyle("ToolbarSeachTextField");
        CancelButtonStyle = GUI.skin.FindStyle("ToolbarSeachCancelButton");
        GroupStyle = GUIStyle.none;
    }

    public static string Draw(string searchValue)
    {
        searchValue = GUILayout.TextField(searchValue, TextFieldStyle);
        if (GUILayout.Button("", CancelButtonStyle))
        {
            searchValue = string.Empty;
            GUI.FocusControl(null);
        }
        return searchValue;
    }

    public static string DrawHorizontal(string searchValue)
    {
        GUILayout.BeginHorizontal(GroupStyle);
        searchValue = Draw(searchValue);
        GUILayout.EndHorizontal();
        return searchValue;
    }

    public static string DrawVertical(string searchValue)
    {
        GUILayout.BeginVertical(GroupStyle);
        searchValue = Draw(searchValue);
        GUILayout.EndVertical();
        return searchValue;
    }
}