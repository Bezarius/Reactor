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

    public T Create()
    {
        Assert.IsNotNull(SettingPath);
        return AssetHelper.CreateSettingAsset<T>(SettingPath);
    }

    public void SetDirty()
    {
        _settings = null;
        AssetHelper.RemoveEmptyAssets(SettingPath);
    }
}