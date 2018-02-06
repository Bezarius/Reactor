using System;
using Reactor.Entities;
using Reactor.Pools;
using UniRx;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.MonoBehaviours
{
    [Serializable]
    public class EntityView : MonoBehaviour
    {
        [SerializeField]
        private string _poolName;

        private IPoolManager _poolManager;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private IEntity _entity;

        public IEntity Entity
        {
            get
            {
                return _entity;
            }
            set
            {
                // возможны ложные срабатывания, нужно проверить
                StopComponentMonitor();
                _entity = value;
                StartComponentMonitor();
            }
        }

        [Inject]
        public void Inject(IPoolManager poolManager)
        {
            _poolManager = poolManager;
        }

        private void Awake()
        {
            if (Entity == null && _poolManager != null)
            {
                var pool = _poolManager.GetPool(_poolName);
                Entity = pool.CreateEntity();
            }
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

        private void StartComponentMonitor()
        {
            //Debug.Log("StartComponentMonitor");
            foreach (var component in Entity.Components)
            {
                if (component != null)
                {
                    this.gameObject.AddComponent(component.WrapperType);
                }
            }
            Entity.OnAddComponent.Subscribe(component =>
            {
                if (this.gameObject.GetComponent(component.WrapperType) == null)
                    this.gameObject.AddComponent(component.WrapperType);
                else Debug.LogWarning(string.Format("Wrapper component '{0}' already added!", component.WrapperType.Name));
            }).AddTo(_compositeDisposable);

            Entity.OnRemoveComponent.Subscribe(component =>
            {
                var wrapper = this.gameObject.GetComponent(component.WrapperType);
                if (wrapper != null)
                    Destroy(wrapper);
            }).AddTo(_compositeDisposable);
        }

        private void StopComponentMonitor()
        {
            //Debug.Log("StopComponentMonitor");
            var components = this.GetComponents(typeof(ComponentWrapper<>));
            foreach (var component in components)
            {
                Destroy(component);
            }
            if (_compositeDisposable.Count > 0)
                _compositeDisposable.Dispose();
        }
    }
}