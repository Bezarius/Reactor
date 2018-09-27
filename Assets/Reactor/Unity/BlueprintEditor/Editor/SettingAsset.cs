using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class SettingAsset<T> where T : ScriptableObject
{
    public string SettingPath { get; set; }

    private List<T> _settings;

    public List<T> Settings
    {
        get
        {
            return _settings ??
                   (_settings = Resources.LoadAll<T>("").ToList());
        }
        set
        {
            _settings = value;
        }
    }

#if UNITY_EDITOR
    public T Create()
    {
        Assert.IsNotNull(SettingPath, "SettingPath != null");
        return AssetHelper.CreateSettingAsset<T>(SettingPath);
    }

    public void SetDirty()
    {
        _settings = null;
        AssetHelper.RemoveEmptyAssets(SettingPath);
    }
#endif
}