using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Components;
using Reactor.Events;
using Reactor.Pools;

namespace Reactor.Entities
{
    public class Entity : IEntity
    {
        private readonly Dictionary<Type, IComponent> _components;

        public IEventSystem EventSystem { get; private set; }

        public int Id { get; private set; }
        public IPool Pool { get; private set; }
        public IEnumerable<IComponent> Components { get { return _components.Values; } }


        public Entity(int id, IPool pool, IEventSystem eventSystem)
        {
            Id = id;
            Pool = pool;
            EventSystem = eventSystem;
            _components = new Dictionary<Type, IComponent>();
        }

        public IComponent AddComponent(IComponent component)
        {
            _components.Add(component.GetType(), component);
            EventSystem.Publish(new ComponentAddedEvent(this, component));
            return component;
        }

        /// <summary>
        /// I highly recommend using AddComponent(new Component())
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponent<T>() where T : class, IComponent, new()
        {
            return (T)AddComponent(new T());
        }

        public void AddComponents(IEnumerable<IComponent> components)
        {
            foreach (var component in components)
            {
                _components.Add(component.GetType(), component);
            }
            EventSystem.Publish(new ComponentsAddedEvent(this, components));
        }

        public void RemoveComponent(IComponent component)
        {
            if (!_components.ContainsKey(component.GetType()))
            {
                return;
            }

            var disposable = component as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

            _components.Remove(component.GetType());
            EventSystem.Publish(new ComponentRemovedEvent(this, component));
        }

        public void RemoveComponent<T>() where T : class, IComponent
        {
            if(!HasComponent<T>()) { return; }

            var component = GetComponent<T>();
            RemoveComponent(component);
        }

        private void RemoveComponents(IEnumerable<IComponent> components)
        {
            components = components.ToArray();
            foreach (var component in components)
            {
                var type = component.GetType();
                if (!_components.ContainsKey(type))
                {
                    continue;
                }

                var disposable = component as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
                _components.Remove(type);
                EventSystem.Publish(new ComponentRemovedEvent(this, component));
            }
            //EventSystem.Publish(new ComponentsRemovedEvent(this, components));
        }

        public void RemoveAllComponents(Func<IComponent, bool> func)
        {
            var components = Components.Where(func).ToArray();
            RemoveComponents(components);
        }

        public void RemoveAllComponents()
        {
            RemoveComponents(Components);
        }

        public T GetComponent<T>(Type t) where T : class, IComponent
        {
            return _components[t] as T;
        }

        public bool HasComponent<T>() where T : class, IComponent
        {
            return _components.ContainsKey(typeof(T));
        }

        public bool HasComponents(params Type[] componentTypes)
        {
            return _components.Count != 0 && componentTypes.All(x => _components.ContainsKey(x));
        }

        public T GetComponent<T>() where T : class, IComponent
        {
            return _components[typeof(T)] as T;
        }

        public void Dispose()
        {
            RemoveAllComponents();
        }
    }
}
