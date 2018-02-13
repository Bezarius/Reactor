using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class AssetHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    public static void CreatePath(string path)
    {
        if (!Directory.Exists(Path.Combine(Application.dataPath, path)))
        {
            var folders = path.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            if (folders.Length > 0 && string.Equals(folders[0], "Assets"))
            {
                var unityPath = folders[0];
                var appPath = Application.dataPath;
                for (int i = 1; i < folders.Length; i++)
                {
                    appPath = Path.Combine(appPath, folders[i]);
                    if (!Directory.Exists(appPath))
                        AssetDatabase.CreateFolder(unityPath, folders[i]);
                    unityPath = Path.Combine(unityPath, folders[i]);
                }
            }
        }
    }

    public static void RemoveEmptyAssets(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists) return;
        foreach (var file in directoryInfo.GetFiles("*.asset", SearchOption.AllDirectories))
        {
            if (file.Length > 42) continue;
            var assetPath = string.Concat("Assets", file.FullName.Substring(Application.dataPath.Length, file.FullName.Length - Application.dataPath.Length));
            AssetDatabase.DeleteAsset(assetPath);
        }
    }

    public static T CreateSettingAsset<T>(string path) where T : ScriptableObject
    {
        var data = ScriptableObject.CreateInstance<T>();
        CreatePath(path);
        var fileName = string.Format("{0}.asset", Guid.NewGuid());
        var fullPath = Path.Combine(path, fileName);
        AssetDatabase.CreateAsset(data, fullPath);
        AssetDatabase.SaveAssets();
        return data;
    }
}