using System.Collections;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Camera")]
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField] private Camera playerCamera;
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(playerCamera).AsSingle().NonLazy();
        }
    }

}