using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Reactor.Components;
using UnityEngine;

public abstract class ComponentContainerScriptableObject : ScriptableObject, ISerializationCallbackReceiver
{
    [HideInInspector]
    [SerializeField]
    private string _state;

    [NonSerialized]
    public List<IComponent> Components = new List<IComponent>();

    public void OnBeforeSerialize()
    {
        var json = JsonConvert.SerializeObject(Components, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        });
        _state = json;
    }

    public void OnAfterDeserialize()
    {
        if (!string.IsNullOrEmpty(_state))
        {
            var result = JsonConvert.DeserializeObject<List<IComponent>>(_state, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            });
            Components = result;
        }
    }
}