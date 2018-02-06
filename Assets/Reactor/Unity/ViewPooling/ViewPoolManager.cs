using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.SceneCollections;
using Reactor.Entities;
using Zenject;

namespace Assets.Reactor.Unity.ViewPooling
{
    public class ViewPoolManager
    {
        private readonly Dictionary<string, IViewPool> _viewPools = new Dictionary<string, IViewPool>();
        private readonly IInstantiator _instantiator;

        public ViewPoolManager(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public void RegisterCollection<T>(string pathToPrefabs, int poolSize = 5, bool withInjection = false)
        {
            if (!typeof(T).IsEnum)
                throw new Exception("Type must be enum!");

            var loader = new PrefabLoader<T>(pathToPrefabs);
            loader.Load();

            var enumList = Enum.GetNames(typeof(T)).ToList();
            for (int i = 0; i < loader.Prefabs.Length; i++)
            {
                IViewPool viewPool;
                if (withInjection)
                    viewPool = new InjectableViewPool(_instantiator, loader.Prefabs[i], poolSize);
                else viewPool = new ViewPool(loader.Prefabs[i], poolSize);
                _viewPools.Add(string.Format(@"{0}{1}", typeof(T).Name, enumList[i]), viewPool);
            }
        }

        public IViewPool GetViewPool<T>(T value)
        {
            if (!typeof(T).IsEnum)
                throw new Exception("Type must be enum!");

            var key = string.Format(@"{0}{1}", typeof(T).Name, value);

            IViewPool viewPool = null;
            if (_viewPools.TryGetValue(key, out viewPool))
                return viewPool;
            throw new Exception(string.Format("ViewPool for '{0}' not found!", value));
        }
    }
}
