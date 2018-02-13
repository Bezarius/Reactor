using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

public class ProjectEditor : EditorWindow
{

    [MenuItem("Window/Blueprint Editor")]
    public static ProjectEditor ShowWindow()
    {
        ProjectEditor window = GetWindow<ProjectEditor>("Blueprint Editor");
        window.minSize = new Vector2(600, 400);
        return window;
    }

    [SerializeField]
    private Rect _menuRect = new Rect(0, 0, 200, 1000);
    private float _minMenuWidth = 150;
    private float _maxMenuWidth = 400;

    [SerializeField]
    private int _menuIndex;

    [SerializeField]
    private string[] _menuItems;

    [SerializeField]
    private List<ModuleMap> _editorMap;

    private void OnEnable()
    {
        RebuildEditorMap();
        _menuItems = _editorMap.OrderBy(y => y.Order).Select(x => x.Menu).ToArray();
    }

    private void OnGUI()
    {
        _menuRect.height = position.height - _menuRect.y;

        GUILayout.BeginArea(_menuRect, GUIStyleHelper.Menu);
        _menuIndex = GUILayout.SelectionGrid(_menuIndex, _menuItems, 1, EditorStyles.toolbarButton);
        GUILayout.EndArea();

        Rect rect = new Rect(_menuRect.width, _menuRect.y, position.width - _menuRect.width, position.height - _menuRect.y);
        GUILayout.BeginArea(rect);
        foreach (ModuleMap map in _editorMap)
        {
            if (map.Menu == _menuItems[_menuIndex] && map.Enabled)
            {
                map.Editor.Position = rect;
                map.Editor.OnGUI();
            }
        }
        GUILayout.EndArea();
        ResizeMenuHorizontal();
    }

    private void Update()
    {
        foreach (var map in _editorMap)
        {
            if (map.Menu == _menuItems[_menuIndex])
            {
                if (!map.Enabled)
                {
                    map.Editor.OnEnable();
                    map.Enabled = true;
                }
                map.Editor.Update();
            }
        }
    }

    private void ResizeMenuHorizontal()
    {
        var rect = new Rect(_menuRect.width - 5, _menuRect.y, 10, _menuRect.height);
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
                    _menuRect.width = ev.mousePosition.x;
                    _menuRect.width = Mathf.Clamp(_menuRect.width, _minMenuWidth, _maxMenuWidth);
                    ev.Use();
                }
                break;
        }
    }

    private void RebuildEditorMap()
    {
        if (_editorMap == null)
        {
            _editorMap = new List<ModuleMap>();
        }
        _editorMap.RemoveAll(x => x.Editor == null);
        var types = GetSubTypes(typeof(ModuleEditor));
        foreach (var type in types)
        {
            var attributes = type.GetCustomAttributes(true);
            foreach (object attribute in attributes)
            {
                var moduleAttribute = attribute as CustomModuleAttribute;

                var type1 = type;
                if (moduleAttribute != null && _editorMap.Find(x => x.Editor.GetType() == type1) == null)
                {
                    var map = new ModuleMap
                    {
                        Menu = moduleAttribute.MenuItem,
                        Editor = (ModuleEditor) CreateInstance(type),
                        Order = moduleAttribute.Order
                    };
                    map.Editor.hideFlags = HideFlags.HideAndDontSave;
                    _editorMap.Add(map);
                }
            }
        }
    }

    public static Type[] GetSubTypes(Type baseType)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(baseType));
        return types.ToArray();
    }

    [System.Serializable]
    private class ModuleMap
    {
        public string Menu;
        public int Order;
        public ModuleEditor Editor;
        public bool Enabled;
    }
}
