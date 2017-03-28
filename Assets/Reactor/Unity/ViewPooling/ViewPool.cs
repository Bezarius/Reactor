using System.Collections.Generic;
using System.Linq;
using Reactor.Extensions;
using UnityEngine;

namespace Assets.Reactor.Unity.ViewPooling
{
    public class ViewPool : IViewPool
    {
        private readonly Stack<GameObject> _pooledObjects = new Stack<GameObject>();

        public GameObject Prefab { get; private set; }
        public int IncrementSize { get; private set; }

        public ViewPool(GameObject prefab, int incrementSize = 5)
        {
            Prefab = prefab;
            IncrementSize = incrementSize;
        }

        public void PreAllocate(int allocationCount)
        {
            for (var i = 0; i < allocationCount; i++)
            {
                var newInstance = Object.Instantiate(Prefab);
                newInstance.SetActive(false);
                _pooledObjects.Push(newInstance);
            }
        }

        public void DeAllocate(int dellocationCount)
        {
            _pooledObjects
                .Take(dellocationCount)
                .ToArray()
                .ForEachRun(x =>
                {
                    _pooledObjects.Pop();
                    Object.Destroy(x);
                });
        }

        public GameObject AllocateInstance()
        {
            if (_pooledObjects.Count == 0)
            {
                PreAllocate(IncrementSize);
            }
            var availableGameObject = _pooledObjects.Pop();
            availableGameObject.SetActive(true);
            return availableGameObject;
        }

        public void ReleaseInstance(GameObject instance)
        {
            _pooledObjects.Push(instance);
            instance.SetActive(false);
        }

        public void EmptyPool()
        {
            _pooledObjects.Clear();
        }
    }
}