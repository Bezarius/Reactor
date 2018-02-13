using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


[CustomModule("Blueprint", 2)]
[Serializable]
public class BlueprintEditor : ListEditor
{
    [SerializeField]
    private SerializedObject _itemObject;
    [SerializeField]
    private Vector2 _scroll;
    [SerializeField]
    private Editor _editor;
    [SerializeField]
    private string _searchString;
    [SerializeField]
    private BlueprintSettings _selected;

    internal sealed override void OnEnable()
    {
        if (_itemObject == null && SettingHelper.BlueprintSettings.Settings.Count > 0)
        {
            _itemObject = new SerializedObject(_selected ?? SettingHelper.BlueprintSettings.Settings[0]);
            _editor = Editor.CreateEditor(_itemObject.targetObject);
        }
    }

    protected override void OnListGUI()
    {
        SelectItem();
    }

    protected override void OnValueGUI()
    {
        DrawItem();
    }

    private void DrawItem()
    {
        GUILayout.Label("");
        if (_itemObject != null && _itemObject.targetObject != null)
        {
            if (_editor == null || _editor.target != _itemObject.targetObject)
            {
                _editor = Editor.CreateEditor(_itemObject.targetObject);
            }
            _editor.OnInspectorGUI();
        }
    }

    private void SelectItem()
    {
        var proptypes = SettingHelper.PrototypeSettings.Settings.Select(x => x.Name).ToArray();
        GUILayout.BeginHorizontal();
        _searchString = ToolbarSearchUI.DrawHorizontal(_searchString);
        if (GUILayout.Button("", GUIStyleHelper.Plus, GUILayout.Height(18), GUILayout.Width(18f)))
        {
            SelectWindow.Show("Select Blueprint", proptypes, AddItem);
        }
        GUILayout.EndHorizontal();

        var items = string.IsNullOrEmpty(_searchString) ?
            SettingHelper.BlueprintSettings.Settings :
            SettingHelper.BlueprintSettings.Settings.Where(x => Regex.IsMatch(x.Name, _searchString, RegexOptions.IgnoreCase)).ToList();
        _scroll = GUILayout.BeginScrollView(_scroll);
        for (int i = 0; i < items.Count; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(items[i].Name))
            {
                GUI.FocusControl("");
                _itemObject = new SerializedObject(items[i]);
                _selected = items[i];
            }

            if (GUILayout.Button("", GUIStyleHelper.Minus, GUILayout.Width(18)))
            {
                DestroyImmediate(items[i], true);
                SettingHelper.BlueprintSettings.Settings.RemoveAll(x => x == null);
                AssetDatabase.SaveAssets();
                SettingHelper.BlueprintSettings.SetDirty();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    private void AddItem(string prototypeName)
    {
        var asset = SettingHelper.BlueprintSettings.Create();
        asset.name = "BlueprintSettings";
        var proto = SettingHelper.PrototypeSettings.Settings.First(x => x.Name == prototypeName);
        asset.Components = proto.Components;
        SettingHelper.BlueprintSettings.Settings.Add(asset);
        GUI.FocusControl("");
        _itemObject = new SerializedObject(asset);
    }
}