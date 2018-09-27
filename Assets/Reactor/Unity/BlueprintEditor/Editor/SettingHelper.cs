public static class SettingHelper
{
    public static readonly SettingAsset<BlueprintSettings> BlueprintSettings =
        new SettingAsset<BlueprintSettings>();

    public static readonly SettingAsset<PrototypeSettings> PrototypeSettings =
        new SettingAsset<PrototypeSettings>();

    static SettingHelper()
    {
        BlueprintSettings.SettingPath = "Assets/ProjectSettings/Resources";
        PrototypeSettings.SettingPath = "Assets/ProjectSettings/Resources";
    }
}