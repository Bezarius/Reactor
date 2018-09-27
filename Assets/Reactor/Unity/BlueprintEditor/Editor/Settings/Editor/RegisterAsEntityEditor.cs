using Assets.Editor;
using Reactor.Components;
using Reactor.Unity.Helpers.EditorInputs;
using Reactor.Unity.MonoBehaviours;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;


[CustomEditor(typeof(RegisterAsEntity))]
[CanEditMultipleObjects]
public class RegisterAsEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var entity = (RegisterAsEntity)target;

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        entity.PoolName = EditorGUILayout.TextField("Pool Name:", entity.PoolName);
        EditorGUILayout.EndHorizontal();

        if (entity.Components != null)
        {
            EditorGUILayout.Space();
            foreach (var component in entity.Components)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.BeginHorizontal();
                var foldoutState = DrawFoldout(component);
                if (PopupButtonClicked())
                    DisplayComponentMenu(entity, component);
                EditorGUILayout.EndHorizontal();
                if (foldoutState)
                    DrawComponent(entity.gameObject, component);
                EditorGUILayout.EndVertical();
            }
            if (AddComponentButtonClicked())
            {
                BuildEntityWindow.Show(entity.gameObject, entity.Components);
            }
        }

        if (GUI.changed)
        {
            Undo.RecordObject(entity, "Entity Editor Modify");
            EditorUtility.SetDirty(entity);
            EditorSceneManager.MarkSceneDirty(entity.gameObject.scene);
        }
    }

    private static bool AddComponentButtonClicked()
    {
        return GUILayout.Button("Add Component", GUI.skin.GetStyle("Button"));
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

    private static void DisplayComponentMenu(RegisterAsEntity entity, IComponent component)
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
                        entity.Components.Remove(component);
                        break;
                }
            }, null);
        evt.Use();
    }
}