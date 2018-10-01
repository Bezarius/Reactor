using System;
using System.Collections.Generic;
using Reactor.Components;
using Reactor.Pools;
using UniRx;

namespace Reactor.Entities
{
    public interface IEntity :  IDisposable
    {
        int Id { get; }
        IPool Pool { get; }
        Groups.ISystemReactor Reactor { get; set; }
        IEnumerable<IComponent> Components { get; }

        T AddComponent<T>(T component) where T : class, IComponent;
        T AddComponent<T>() where T : class, IComponent, new();

        void RemoveComponent<T>() where T : class, IComponent;
        void RemoveComponent<T>(T component) where T : class, IComponent;

        void RemoveAllComponents(Func<IComponent, bool> func);
        void RemoveAllComponents();

        T GetComponent<T>() where T : class, IComponent;

        bool HasComponent<T>() where T : class, IComponent;
        bool HasComponents(params Type[] component);

        void Destory();

        IReactiveCommand<IComponent> OnAddComponent { get; }
        IReactiveCommand<IComponent> OnRemoveComponent { get; }
    }
}
