using System.Collections.Generic;
using ModestTree;
using Reactor.Systems;
using UnityEngine;
using Zenject;

namespace Reactor.Unity.Installers
{
    /// <summary>
    /// This is for just binding systems and not registering them
    /// </summary>
    public class AutoBindSystemsInstaller : MonoInstaller
    {
        public List<string> SystemNamespaces = new List<string>();

        public override void InstallBindings()
        {
            SystemNamespaces.Insert(0, "Reactor.Unity.Systems");
            Container.Bind<ISystem>()
                .To(x => x.AllTypes()
                    .Where(type => SystemNamespaces.Contains(type.Namespace))
                    .Where(type => !type.IsInterface && !type.IsAbstract)
                    .Where(type => type == typeof(ISystem) || type.DerivesFrom<ISystem>())
                    .Where(type =>
                    {
                        Debug.Log(type.FullName);
                        return true;
                    })
                )
                .AsSingle();
        }
    }
}