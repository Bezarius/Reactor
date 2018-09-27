using System.Collections.Generic;
using Reactor.Systems;
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
            Container.Bind<ISystem>().To(x => x.AllTypes().Where(z => !z.IsAbstract).DerivingFrom<ISystem>().InNamespaces(SystemNamespaces)).AsSingle();
            Container.Bind(x => x.AllTypes().Where(z => !z.IsAbstract).DerivingFrom<ISystem>().InNamespaces(SystemNamespaces)).AsSingle();
        }
    }
}