using UnityEngine;
using UnityEditor;
using System;

[CustomModule("Prototype", 1)]
[Serializable]
public class PrototypeEditor : ListEditor
{
    [SerializeField]
    private SerializedObject _itemObject;

    [SerializeField]
    private Vector2 _scroll;

    [SerializeField]
    private Editor _editor;

    [SerializeField]
    private PrototypeSettings _selected;

    internal sealed override void OnEnable()
    {
        if (_itemObject == null && SettingHelper.PrototypeSettings.Settings.Count > 0)
        {
            _itemObject = new SerializedObject(_selected ?? SettingHelper.PrototypeSettings.Settings[0]);
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
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("", GUIStyleHelper.Plus))
        {
            AddItem();
        }
        GUILayout.EndHorizontal();

        var items = SettingHelper.PrototypeSettings.Settings;

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
                SettingHelper.PrototypeSettings.Settings.RemoveAll(x => x == null);
                AssetDatabase.SaveAssets();
                SettingHelper.PrototypeSettings.SetDirty();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    private void AddItem()
    {
        var asset = SettingHelper.PrototypeSettings.Create();
        asset.name = "PrototypeSettings";
        SettingHelper.PrototypeSettings.Settings.Add(asset);
        GUI.FocusControl("");
        _itemObject = new SerializedObject(asset);
    }
}
