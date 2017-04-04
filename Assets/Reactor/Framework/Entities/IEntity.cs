using System;
using System.Collections.Generic;
using Reactor.Components;
using Reactor.Pools;

namespace Reactor.Entities
{
    public interface IEntity : IDisposable
    {
        int Id { get; }
        //Groups.SystemReactor Reactor { get; set; }
        IPool Pool { get; }
        IEnumerable<IComponent> Components { get; }

        IComponent AddComponent(IComponent component);
        T AddComponent<T>() where T : class, IComponent, new();
        void AddComponents(IEnumerable<IComponent> components);

        void RemoveComponent(IComponent component);
        void RemoveComponent<T>() where T : class, IComponent;
        void RemoveAllComponents(Func<IComponent, bool> func);
        void RemoveAllComponents();

        T GetComponent<T>() where T : class, IComponent;
        T GetComponent<T>(Type t) where T : class, IComponent;

        bool HasComponent<T>() where T : class, IComponent;
        bool HasComponents(params Type[] component);
    }
}
