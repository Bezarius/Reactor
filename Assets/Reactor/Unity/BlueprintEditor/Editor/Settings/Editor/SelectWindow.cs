using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class SelectWindow : EditorWindow
{
    private string _searchString;
    private Vector2 _scrollPosition;
    private string[] _options;
    private Action<string> _selectAction;

    private void OnGUI()
    {
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height - 16));
        Draw();
        GUILayout.EndScrollView();
    }


    private void Draw()
    {
        _searchString = ToolbarSearchUI.DrawHorizontal(_searchString);

        foreach (var option in string.IsNullOrEmpty(_searchString) ? _options : _options.Where(x => Regex.IsMatch(x, _searchString, RegexOptions.IgnoreCase)))
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            if (GUILayout.Button(option, GUIStyle.none, GUILayout.Height(20f), GUILayout.Width(position.width - 15f)))
            {
                _selectAction(option);
                Close();
            }
            GUILayout.EndHorizontal();
        }
    }

    public static void Show(string title, string [] options, Action<string> selectAction)
    {
        var window = GetWindow<SelectWindow>(true, title);
        window.Init(options, selectAction);
    }

    private void Init(string[] options, Action<string> selectAction)
    {
        _options = options;
        _selectAction = selectAction;
    }
}