using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Reactor.Components;
using Reactor.Pools;
using Reactor.Unity.Components;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.MonoBehaviours
{
    [Serializable, DisallowMultipleComponent]
    public class RegisterAsEntity : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] public string PoolName;

        [HideInInspector] [SerializeField] private string _state;

        public List<IComponent> Components = new List<IComponent>();

        private IPoolManager _poolManager;

        [Inject]
        public void Inject(IPoolManager poolManager)
        {
            _poolManager = poolManager;
        }

        private void Awake()
        {
            var pool = _poolManager.GetPool(PoolName);
            var view = Components.FirstOrDefault(x => x.Type == typeof(ViewComponent));
            if (view == null)
            {
                view = new ViewComponent();
                Components.Add(view);
            }
            (view as ViewComponent).GameObject = this.gameObject;
            var entity = pool.CreateEntity((IEnumerable<IComponent>)Components);
            var entityView = this.gameObject.AddComponent<EntityView>();
            entityView.Entity = entity;
            Destroy(this);
        }

        #region Serialization

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public void OnBeforeSerialize()
        {
            var json = JsonConvert.SerializeObject(Components, Formatting.Indented, JsonSerializerSettings);
            _state = json;
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(_state))
            {
                var result = JsonConvert.DeserializeObject<List<IComponent>>(_state, JsonSerializerSettings);
                Components = result;
            }
        }

        #endregion

    }
}