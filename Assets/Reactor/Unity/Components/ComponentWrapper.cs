using System;
using Reactor.Components;
using Reactor.Unity.MonoBehaviours;
using UnityEngine;

public interface IComponentContainer
{
    void Setup();
    IComponent Component { get; }
}


[Serializable]
[DisallowMultipleComponent]
public class ComponentWrapper<TComponent> : MonoBehaviour, IComponentContainer where TComponent : class, IComponent, new()
{
    private bool _isApplicationQuitting = false;

    [SerializeField]
    private TComponent _component = new TComponent();

    public IComponent Component
    {
        get { return _component; }
        set { _component = (TComponent)value; }
    }

    public void Setup()
    {
        EntityView view = GetComponent<EntityView>();
        if (view && view.Entity != null)
        {
            if (!view.Entity.HasComponent<TComponent>())
                view.Entity.AddComponent(_component);
            else
                _component = view.Entity.GetComponent<TComponent>();
        }
        Initialize();
    }


    protected virtual void Initialize()
    {
    }

    private void OnDisable()
    {
        EntityView view = GetComponent<EntityView>();
        if (view && !_isApplicationQuitting)
        {
            if (view.Entity != null && view.Entity.HasComponent<TComponent>())
                view.Entity.RemoveComponent<TComponent>();
            _component = null;
        }
    }

    private void OnApplicationQuit()
    {
        _isApplicationQuitting = true;
    }
}