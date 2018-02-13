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
        [SerializeField]
        private string _poolName;

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private IEntity _entity;
        private bool _monitorEnabled;

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
            _monitorEnabled = true;
            foreach (var component in Entity.Components)
            {
                if (component != null && component.WrapperType != null && this.gameObject.GetComponent(component.WrapperType) == null)
                {
                    this.gameObject.AddComponent(component.WrapperType);
                }
            }
            Entity.OnAddComponent.Subscribe(component =>
            {
                if (component.WrapperType != null)
                {
                    if (this.gameObject.GetComponent(component.WrapperType) == null)
                        this.gameObject.AddComponent(component.WrapperType);
                }
            }).AddTo(_compositeDisposable);

            Entity.OnRemoveComponent.Subscribe(component =>
            {
                if (component.WrapperType != null)
                {
                    var wrapper = this.gameObject.GetComponent(component.WrapperType);
                    if (wrapper != null)
                        Destroy(wrapper);
                }
            }).AddTo(_compositeDisposable);
        }

        private void StopComponentMonitor()
        {
            if (_monitorEnabled)
            {
                var components = this.GetComponents(typeof(MonoBehaviour))
                    .Where(x => x.GetType().IsAssignableFrom(typeof(IComponent)))
                    .ToList();
                foreach (MonoBehaviour component in components)
                {
                    component.enabled = false;
                }
                if (_compositeDisposable.Count > 0)
                    _compositeDisposable.Dispose();
                _monitorEnabled = false;
            }
        }
    }
}