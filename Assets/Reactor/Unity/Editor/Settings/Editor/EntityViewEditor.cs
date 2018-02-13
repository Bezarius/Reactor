using Reactor.Components;
using Reactor.Unity.Helpers.EditorInputs;
using Reactor.Unity.MonoBehaviours;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EntityView))]
[CanEditMultipleObjects]
public class EntityViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var entityView = (EntityView)target;

        if (!EditorApplication.isPlaying)
        {
            DestroyImmediate(entityView);
            return;
        }

        if (entityView.Entity != null && entityView.Entity.Components != null)
        {
            EditorGUILayout.Space();
            foreach (var component in entityView.Entity.Components)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.BeginHorizontal();
                var foldoutState = DrawFoldout(component);
                if (PopupButtonClicked())
                    DisplayComponentMenu(entityView, component);
                EditorGUILayout.EndHorizontal();
                if (foldoutState)
                    DrawComponent(entityView.gameObject, component);
                EditorGUILayout.EndVertical();
            }
        }

        if (GUI.changed)
        {
            Undo.RecordObject(entityView, "Entity Editor Modify");
            //EditorUtility.SetDirty(entityView);
            //EditorSceneManager.MarkSceneDirty(entityView.gameObject.scene);
        }
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

    private static void DrawComponent(GameObject target, IComponent component)
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
            handler.DrawComponentElementUI(target, component, property);
            EditorGUILayout.EndHorizontal();
        }
    }

    private static void DisplayComponentMenu(EntityView entityView, IComponent component)
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
                        entityView.Entity.RemoveComponent(component);
                        break;
                }
            }, null);
        evt.Use();
    }
}