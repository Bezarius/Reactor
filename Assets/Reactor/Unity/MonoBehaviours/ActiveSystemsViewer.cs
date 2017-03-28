using Reactor.Systems.Executor;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.Helpers
{
    public class ActiveSystemsViewer : MonoBehaviour
    {
        [Inject]
        public ISystemExecutor SystemExecutor { get; private set; }
    }
}