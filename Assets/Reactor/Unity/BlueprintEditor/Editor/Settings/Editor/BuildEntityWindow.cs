using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Reactor.Components;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [Flags]
    public enum EntityWindowTabs
    {
        Components = 0x1,
        Blueprints = 0x2
    }

    public class BuildEntityWindow : EditorWindow
    {
        private string _searchString;

        private Vector2 _scrollPosition;

        private GameObject _target;
        private List<IComponent> _entityComponents;

        private readonly List<Type> _possibleComponents = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IComponent).IsAssignableFrom(p) && p.IsClass && p.ContainsGenericParameters == false)
            .ToList();

        private int _tabIndex;
        private EntityWindowTabs _tabs;

        private void OnGUI()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height - 16));
            DrawTabs();
            GUILayout.EndScrollView();
        }

        private void DrawTab(int tabId)
        {
            switch (tabId)
            {
                case (int)EntityWindowTabs.Components:
                    DrawComponentsUI();
                    break;
                case (int)EntityWindowTabs.Blueprints:
                    DrawBlueprintUI();
                    break;
                default: break;
            }
        }

        private void DrawTabs()
        {
            var tabs = _tabs.MaskToStringArray();
            int tabId;
            if (tabs.Length > 1)
            {
                _tabIndex = GUILayout.Toolbar(_tabIndex, tabs);
                tabId = (int)(EntityWindowTabs)Enum.Parse(typeof(EntityWindowTabs), tabs[_tabIndex]);
            }
            else
                tabId = (int)_tabs;

            DrawTab(tabId);
        }

        private void DrawBlueprintUI()
        {
            _searchString = ToolbarSearchUI.DrawHorizontal(_searchString);

            foreach (var blueprintSetting in SettingHelper.BlueprintSettings.Settings)
            {
                GUILayout.BeginHorizontal(GUI.skin.box);
                if (GUILayout.Button(blueprintSetting.Name, GUIStyle.none, GUILayout.Height(20f), GUILayout.Width(position.width - 15f)))
                {
                    try
                    {
                        _entityComponents.Clear();
                        _entityComponents.AddRange(blueprintSetting.Components.DeepClone());

                        // add component wrappers to gameObject
                        if (_target != null && _entityComponents != null)
                        {
                            foreach (var entityComponent in _entityComponents.Where(x => x.WrapperType != null))
                            {
                                if (_target.GetComponent(entityComponent.WrapperType) == null)
                                    _target.AddComponent(entityComponent.WrapperType);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        throw;
                    }
                    Close();
                }
                GUILayout.EndHorizontal();
            }
        }

        private void DrawComponentsUI()
        {
            _searchString = ToolbarSearchUI.DrawHorizontal(_searchString);
            var dict = new Dictionary<string, Type>();
            if (string.IsNullOrEmpty(_searchString))
            {
                foreach (var componentType in _possibleComponents)
                    dict.Add(componentType.FullName, componentType);
            }
            else
            {
                foreach (var component in _possibleComponents.Where(x => Regex.IsMatch(x.Name, _searchString, RegexOptions.IgnoreCase)))
                    dict.Add(component.Name, component);
            }

            foreach (var component in _entityComponents)
                dict.Remove(component.Type.Name);

            foreach (var key in dict.Keys)
            {
                GUILayout.BeginHorizontal(GUI.skin.box);
                if (GUILayout.Button(key, GUIStyle.none, GUILayout.Height(20f), GUILayout.Width(position.width - 15f)))
                {
                    try
                    {
                        var component = Activator.CreateInstance(dict[key]) as IComponent;
                        _entityComponents.Add(component);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        throw;
                    }
                    Close();
                }
                GUILayout.EndHorizontal();
            }
        }

        public static void Show(GameObject target, List<IComponent> components, EntityWindowTabs tabs = EntityWindowTabs.Blueprints | EntityWindowTabs.Components)
        {
            var window = GetWindow<BuildEntityWindow>(true, "Build Entity");
            window.Init(target, components, tabs);
        }

        private void Init(GameObject target, List<IComponent> components, EntityWindowTabs tabs)
        {
            _target = target;
            _entityComponents = components;
            _tabs = tabs;
        }
    }
}