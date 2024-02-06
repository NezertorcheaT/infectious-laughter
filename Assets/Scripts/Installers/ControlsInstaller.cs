using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Controls")]
    public class ControlsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var controls = new Controls();
            controls.Enable();
            Container.Bind<Controls>().FromInstance(controls).AsSingle().NonLazy();
        }
    }
}