using System.Linq;
using Assets.Editor;
using Reactor.Components;
using Reactor.Unity.Helpers.EditorInputs;
using Reactor.Unity.MonoBehaviours;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlueprintSettings))]
[CanEditMultipleObjects]
public class BlueprintSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var blueprint = (BlueprintSettings)target;

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        blueprint.Name = EditorGUILayout.TextField("Name:", blueprint.Name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        blueprint.Object = EditorGUILayout.ObjectField("Object:", blueprint.Object, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

        if (blueprint.Components != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Components:");
            if (AddComponentButtonClicked())
                BuildEntityWindow.Show(null, blueprint.Components, EntityWindowTabs.Components);
            EditorGUILayout.EndHorizontal();
            foreach (var component in blueprint.Components)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.BeginHorizontal();
                var foldoutState = DrawFoldout(component);
                if (PopupButtonClicked())
                    DisplayComponentMenu(blueprint, component);
                EditorGUILayout.EndHorizontal();
                if (foldoutState)
                    DrawComponent(blueprint.Object, component);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();
        if (blueprint.Object != null && GUILayout.Button("Apply", GUILayout.Height(20f), GUILayout.Width(80f)))
        {
            var entity = blueprint.Object.GetComponent<RegisterAsEntity>() ?? blueprint.Object.AddComponent<RegisterAsEntity>();
            entity.Components = blueprint.Components;

            // add component wrappers to gameObject
            if (blueprint.Components != null)
                foreach (var entityComponent in blueprint.Components.Where(x => x.WrapperType != null))
                {
                    if (blueprint.Object.GetComponent(entityComponent.WrapperType) == null)
                        blueprint.Object.AddComponent(entityComponent.WrapperType);
                }
        }

        if (GUI.changed)
        {
            Undo.RecordObject(blueprint, "Entity Editor Modify");
            EditorUtility.SetDirty(blueprint);
        }
    }

    private static bool AddComponentButtonClicked()
    {
        return GUILayout.Button("", GUIStyleHelper.Plus, GUILayout.Height(10), GUILayout.Width(25f));
    }

    private static bool PopupButtonClicked()
    {
        var buttonImage = EditorGUIUtility.FindTexture("_Popup");
        return GUILayout.Button(buttonImage, GUI.skin.GetStyle("Label"), GUILayout.MinWidth(20), GUILayout.Width(20));
    }

    private static bool DrawFoldout(IComponent component)
    {
        component.IsCollapsed = EditorGUILayout.Foldout(component.IsCollapsed, new GUIContent(component.Type.Name));
        return component.IsCollapsed;
    }

    private static void DrawComponent(GameObject gameObject, IComponent component)
    {
        var componentProperties = component.FieldInfos;

        foreach (var property in componentProperties)
        {


            var propertyType = property.FieldType;

            var handler = DefaultEditorInputRegistry.GetHandlerFor(propertyType);

            if (handler == null)
            {
                Debug.LogWarning("This type is not supported: " + propertyType.Name + " - In component: " +
                                 component.Type.Name);
                continue;
            }

            EditorGUILayout.BeginHorizontal();
            handler.DrawComponentElementUI(gameObject, component, property);
            EditorGUILayout.EndHorizontal();
        }
    }

    private static void DisplayComponentMenu(BlueprintSettings settings, IComponent component)
    {
        var evt = Event.current;
        var mousePos = evt.mousePosition;
        EditorUtility.DisplayCustomMenu(
            new Rect(mousePos.x, mousePos.y, 0, 0),
            new[]
            {
                // todo: copy\paste
                new GUIContent("Remove Component"),
            },
            -1,
            (data, options, selected) =>
            {
                switch (options[selected])
                {
                    case "Remove Component":
                        settings.Components.Remove(component);
                        break;
                }
            }, null);
        evt.Use();
    }
}