using Reactor.Pools;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.Helpers
{
    public class PoolManagerViewer : MonoBehaviour
    {
         [Inject]
         public IPoolManager PoolManager { get; private set; }
    }
}