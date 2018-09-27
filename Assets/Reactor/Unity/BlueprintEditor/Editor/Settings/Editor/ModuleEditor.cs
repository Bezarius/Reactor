using UnityEngine;

[System.Serializable]
public abstract class ModuleEditor : ScriptableObject
{
    internal Rect Position;

    internal abstract void OnEnable();

    internal abstract void OnGUI();

    internal virtual void Update(){}
}