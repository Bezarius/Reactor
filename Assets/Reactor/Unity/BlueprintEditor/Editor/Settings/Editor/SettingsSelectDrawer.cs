using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomPropertyDrawer(typeof(SettingsSelectAttribute), true)]
public class SettingsSelectDrawer : PropertyDrawer
{
    private Rect _position;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        this._position = position;
        var settings = SettingHelper.PrototypeSettings.Settings.Where(x => x.GetType() == this.fieldInfo.FieldType).Select(y => y.Name).ToList();
        if (settings.Count > 0)
        {
            int index = 0;
            if (property.objectReferenceValue != null)
            {
                var moduleSettings = property.objectReferenceValue as PrototypeSettings;
                index = settings.FindIndex(x => x == moduleSettings.Name);
            }
            index = EditorGUI.Popup(position, "Settings", index, settings.ToArray());
            property.objectReferenceValue =
                SettingHelper.PrototypeSettings.Settings.FirstOrDefault(x => x.Name == settings[index]);
        }
        else
        {
            EditorGUI.HelpBox(position, fieldInfo.FieldType.Name + " not found, please create one using the ProjectEditor.", MessageType.Error);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var settings = SettingHelper.PrototypeSettings.Settings.Where(x => x.GetType() == this.fieldInfo.FieldType).Select(y => y.Name).ToList();
        var height = ((GUIStyle)"HelpBox").CalcHeight(new GUIContent(fieldInfo.FieldType.Name + " not found, please create one using the ProjectEditor."), _position.width);
        return settings.Count > 0 ? base.GetPropertyHeight(property, label) : Mathf.Clamp(height, height + 17, 1000);
    }
}