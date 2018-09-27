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

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private IEntity _entity;
        private bool _monitorEnabled;

        public readonly IReactiveCommand<IEntity> OnEntityUpdate = new ReactiveCommand<IEntity>();

        public IEntity Entity
        {
            get
            {
                return _entity;
            }
            set
            {
                StopComponentMonitor();
                _entity = value;
                StartComponentMonitor();
                OnEntityUpdate.Execute(_entity);
            }
        }

        private void Awake()
        {
            if (!this.gameObject.IsEntity())
                this.gameObject.SetEntityTag();
        }

        private void OnEnable()
        {
            if (Entity != null)
                StartComponentMonitor();
        }

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

        private void OnDestroy()
        {
            if (Entity != null)
                Entity.Pool.RemoveEntity(this.Entity);
            this.gameObject.SetActive(false);
        }
    }
}