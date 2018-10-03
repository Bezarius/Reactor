using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Components;
using Reactor.Groups;
using Reactor.Pools;
using UniRx;

namespace Reactor.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; }
        public IPool Pool { get; }

        public ISystemReactor Reactor
        {
            get => _reactor;
            set => _reactor = (SystemReactor)value;
        }

        public IEnumerable<IComponent> Components => _components;

        // todo: подумать как можно реализовать этот функционал более корректным способом
        // это удобная, но не самая лучшая реализация, т.к. команды могут быть вызваны вне сущности,
        // а это может привести к некорректному поведению EntityView и других потребителей данного функционала
        // Замена нужна реактивная, т.к. не реактивная реализация будет мотивировать на смешивание стилей 
        private readonly ReactiveCommand<IComponent> _addComponentSubject = new ReactiveCommand<IComponent>();
        public IReactiveCommand<IComponent> OnAddComponent => _addComponentSubject;

        private readonly ReactiveCommand<IComponent> _removeComponentSubject = new ReactiveCommand<IComponent>();
        public IReactiveCommand<IComponent> OnRemoveComponent => _removeComponentSubject;

        private SystemReactor _reactor;
        private readonly List<IComponent> _components;

        public Entity(int id, IEnumerable<IComponent> components, IPool pool, ISystemReactor reactor)
        {
            _reactor = (SystemReactor)reactor;
            Id = id;
            Pool = pool;
            _components = components.ToList();
        }

        public T AddComponent<T>(T component) where T : class, IComponent
        {
            var idx = _reactor.GetFutureComponentIdx(component);
            _components.Insert(idx, component);
            _reactor.AddComponent(this, component);
            OnAddComponent.Execute(component);
            return component;
        }

        /// <summary>
        /// I highly recommend using AddComponent(new Component())
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponent<T>() where T : class, IComponent, new()
        {
            return AddComponent(new T());
        }

        public void RemoveComponent<T>() where T : class, IComponent
        {
            var typeId = TypeCache<T>.TypeId;
            var idx = _reactor.GetComponentIdx(typeId);
            if (_components.Count > idx && _components[idx] != null)
            {
                var component = _components[idx];
                _reactor.RemoveComponent(this, component);
                _components.RemoveAt(idx);

                var disposable = component as IDisposable;

                disposable?.Dispose();

                OnRemoveComponent.Execute(component);
            }
        }

        public void RemoveComponent<T>(T component) where T : class, IComponent
        {
            var typeId = TypeCache<T>.TypeId;
            var idx = _reactor.GetComponentIdx(typeId);

            if (_components.Count > idx && idx >= 0 && _components[idx] != null)
            {
                _components.RemoveAt(idx);
            }

            var disposable = component as IDisposable;

            disposable?.Dispose();

            _reactor.RemoveComponent(this, component);
            OnRemoveComponent.Execute(component);
        }

        private void RemoveComponents(IEnumerable<IComponent> components)
        {
            var componentArray = components.ToArray();
            for (int i = 0; i < componentArray.Length; i++)
            {
                var component = componentArray[i];
                if (component != null)
                {
                    var typeId = component.TypeId;
                    var idx = _reactor.GetComponentIdx(typeId);

                    if (idx >= 0 && idx > _components.Count || _components[idx] == null)
                    {
                        continue;
                    }

                    var disposable = component as IDisposable;

                    disposable?.Dispose();
                    _reactor.RemoveComponent(this, component);
                    _components.RemoveAt(idx);
                    OnRemoveComponent.Execute(component);
                }
            }
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

        public bool HasComponent<T>() where T : class, IComponent
        {
            var typeId = TypeCache<T>.TypeId;
            return _reactor.HasComponentIndex(typeId); // && _components[idx] != null;
        }

        public bool HasComponents(params Type[] componentTypes)
        {
            if (_components.Count > 0)
            {
                for (var i = 0; i < componentTypes.Length; i++)
                {
                    var type = componentTypes[i];
                    var typeId = TypeHelper.GetTypeId(type);
                    var idx = _reactor.GetComponentIdx(typeId);
                    if (idx == -1)
                        return false;
                }
            }
            return true;
        }

        public T GetComponent<T>() where T : class, IComponent
        {
            var typeId = TypeCache<T>.TypeId;
            var idx = _reactor.ComponentIndex.GetTypeIndex(typeId);
            return _components[idx] as T;
        }

        public void Destory()
        {
            this.Pool.RemoveEntity(this);
        }

        public void Dispose()
        {
            RemoveAllComponents();
        }
    }
}
