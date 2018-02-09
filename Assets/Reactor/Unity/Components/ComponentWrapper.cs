using System;
using Reactor.Components;
using Reactor.Unity.MonoBehaviours;
using UnityEngine;

// todo: add auto execution order

public interface IComponentContainer
{
    IComponent Component { get; }
}

[Serializable]
[DisallowMultipleComponent]
public class ComponentWrapper<TComponent> : MonoBehaviour, IComponentContainer where TComponent : class, IComponent, new()
{
    [SerializeField]
    private TComponent _component = new TComponent();

    public IComponent Component
    {
        get { return _component; }
        set { _component = (TComponent)value; }
    }

    protected virtual void Initialize() { }

    private void OnEnable()
    {
        EntityView view = GetComponent<EntityView>();
        if (view && view.Entity != null)
        {
            if (!view.Entity.HasComponent<TComponent>())
                view.Entity.AddComponent(_component);
            else
                _component = view.Entity.GetComponent<TComponent>();
            Initialize();
        }
    }

    private void OnDisable()
    {
        EntityView view = GetComponent<EntityView>();
        if (view)
        {
            if (view.Entity != null && view.Entity.HasComponent<TComponent>())
                view.Entity.RemoveComponent<TComponent>();
        }
    }
}