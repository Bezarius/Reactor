using System;
using System.Linq;
using Reactor.Components;
using Reactor.Entities;
using UniRx;
using UnityEngine;

namespace Reactor.Unity.MonoBehaviours
{
    public interface IEntityView
    {
        IEntity Entity { get; set; }
    }

    [Serializable]
    public class EntityView : MonoBehaviour, IEntityView
    {
#if UNITY_EDITOR
        public bool SystemInfoIsCollapsed { get; set; }
#endif

        [SerializeField]
        private string _poolName;

#if REACTOR_SUPPORT_COMPONENT_WRAPPERS
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private bool _monitorEnabled;
#endif
        private IEntity _entity;
        
        public readonly IReactiveCommand<IEntity> OnEntityUpdate = new ReactiveCommand<IEntity>();

        public IEntity Entity
        {
            get
            {
                return _entity;
            }
            set
            {
#if REACTOR_SUPPORT_COMPONENT_WRAPPERS
                StopComponentMonitor();
#endif
                _entity = value;
#if REACTOR_SUPPORT_COMPONENT_WRAPPERS
                StartComponentMonitor();
#endif
                OnEntityUpdate.Execute(_entity);
            }
        }

        private void Awake()
        {
            if (!this.gameObject.IsEntity())
                this.gameObject.SetEntityTag();
        }

#if REACTOR_SUPPORT_COMPONENT_WRAPPERS
        private void OnEnable()
        {
            if (Entity != null)
                StartComponentMonitor();
    }
#endif

#if REACTOR_SUPPORT_COMPONENT_WRAPPERS
        private void OnDisable()
        {

            StopComponentMonitor();
        }

        private void AddOrEnableWrapper(Type wrapperType)
        {
            var wrapper = this.gameObject.GetComponent(wrapperType) as MonoBehaviour;
            if (wrapper == null)
                wrapper = this.gameObject.AddComponent(wrapperType) as MonoBehaviour;
            if (wrapper.enabled == false)
                wrapper.enabled = true;
            var wrapperSetup = wrapper as IComponentContainer;
            if (wrapperSetup != null)
                wrapperSetup.Setup();

        }

        private void DisableWrapper(Type wrapperType)
        {
            var wrapper = this.gameObject.GetComponent(wrapperType) as MonoBehaviour;
            if (wrapper != null)
                wrapper.enabled = false;
        }

        private void StartComponentMonitor()
        {
            if (_entity != null)
            {
                _monitorEnabled = true;
                foreach (var component in Entity.Components)
                {
                    if (component != null && component.WrapperType != null)
                        AddOrEnableWrapper(component.WrapperType);
                }
                Entity.OnAddComponent.Subscribe(component =>
                {
                    if (component.WrapperType != null)
                        AddOrEnableWrapper(component.WrapperType);
                }).AddTo(_compositeDisposable);

                Entity.OnRemoveComponent.Subscribe(component =>
                {
                    if (component.WrapperType != null)
                        DisableWrapper(component.WrapperType);
                }).AddTo(_compositeDisposable);
            }
        }

        private void StopComponentMonitor()
        {
            if (_monitorEnabled)
            {
                var components = this.GetComponents(typeof(MonoBehaviour))
                    .Where(x => x.GetType().IsAssignableFrom(typeof(IComponent)))
                    .ToList();
                foreach (MonoBehaviour component in components)
                    component.enabled = false;

                if (_compositeDisposable.Count > 0)
                    _compositeDisposable.Dispose();
                _monitorEnabled = false;
            }
        }
#endif

        private void OnDestroy()
        {
            Entity?.Destory();
            this.gameObject.SetActive(false);
        }
    }
}