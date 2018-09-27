using System;
using UnityEngine;


[Serializable]
public class BlueprintSettings : ComponentContainerScriptableObject
{
    [SerializeField]
    public string Name;

    [SerializeField]
    public GameObject Object;
}