using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Signal Bus")]
    public class LocalSignalBusInstaller : MonoInstaller
    {
        public override void InstallBindings() => SignalBusInstaller.Install(Container);
    }
}