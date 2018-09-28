using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Reactor.Components;
using Reactor.Groups;
using Reactor.Pools;
using UniRx;

namespace Reactor.Entities
{

    public static class TypeHelper
    {
        public static int Counter { get; private set; }

        private static readonly Dictionary<Type, int> TypeDict = new Dictionary<Type, int>();

        public static int GetTypeId(Type type)
        {
            int idx;
            if (!TypeDict.TryGetValue(type, out idx))
                idx = Initialize(type);
            return idx;
        }

        public static int Initialize(Type type)
        {
            Counter++;
#if DEBUG
            if (TypeDict.ContainsKey(type))
                throw new Exception(string.Format(@"Type '{0}' is already initialized", type));
#endif
            TypeDict[type] = Counter;
            return Counter;
        }

        static TypeHelper()
        {
            var assignFrom = typeof(IComponent);

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(t => assignFrom.IsAssignableFrom(t) && !t.IsAbstract).ToList();

            var tt = typeof(TypeCache<>);

            foreach (var type in types)
            {
                var args = new[] { type };
                var cache = tt.MakeGenericType(args);
                var field = cache.GetField("TypeId", BindingFlags.Static | BindingFlags.Public);
                var result = field.GetValue(null);
                //Debug.Log(result);
                //cache.GetMethod("")
            }
        }
    }

    public static class TypeCache<T>
    {
        public static readonly Type Type;

        public static readonly int TypeId;

        static TypeCache()
        {
            Type = typeof(T);
            if (Type == null)
                throw new Exception("Incorrect type initialization!");
            TypeId = TypeHelper.Initialize(Type);
        }
    }

    public class Entity : IEntity
    {
        public int Id { get; private set; }

        public ISystemReactor Reactor
        {
            get { return _reactor; }
            set { _reactor = value; }
        }

        public IPool Pool { get; private set; }
        public IEnumerable<IComponent> Components { get { return _components; } }

        // todo: подумать как можно реализовать этот функционал более корректным способом
        // это удобная, но не самая лучшая реализация, т.к. команды могут быть вызваны вне сущности,
        // а это может привести к некорректному поведению EntityView и других потребителей данного функционала
        // Замена нужна реактивная, т.к. не реактивная реализация будет мотивировать на смешивание стилей 
        private readonly ReactiveCommand<IComponent> _addComponentSubject = new ReactiveCommand<IComponent>();
        public IReactiveCommand<IComponent> OnAddComponent { get { return _addComponentSubject; } }

        private readonly ReactiveCommand<IComponent> _removeComponentSubject = new ReactiveCommand<IComponent>();
        public IReactiveCommand<IComponent> OnRemoveComponent { get { return _removeComponentSubject; } }

        private ISystemReactor _reactor;
        private readonly List<IComponent> _components;

        public Entity(int id, IEnumerable<IComponent> components, IPool pool, ISystemReactor reactor)
        {
            _reactor = reactor;
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

        public void AddComponents(IEnumerable<IComponent> components)
        {
            throw new NotImplementedException();
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

                if (disposable != null)
                {
                    disposable.Dispose();
                }

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

            if (disposable != null)
            {
                disposable.Dispose();
            }

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

                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
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
            //var type = typeof(T);
            //Assert.IsTrue(_components.ContainsKey(type), string.Format(@"Entity with id: '{0}', doesn't contain '{1}' component", this.Id, type.Name));
            var typeId = TypeCache<T>.TypeId;
            var idx = _reactor.GetComponentIdx(typeId);
            if (idx >= 0) //todo: try optimize because every check slowdown system
                return _components[idx] as T;
            return null;
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
