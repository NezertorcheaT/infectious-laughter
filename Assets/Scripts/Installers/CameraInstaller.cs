using Cinemachine;
using Inventory;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Camera")]
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField] private CinemachineVirtualCamera playerCinemachineCamera;
        public override void InstallBindings()
        {
            Container.Bind<CinemachineVirtualCamera>().FromInstance(playerCinemachineCamera).AsSingle().NonLazy();
        }
    }
}