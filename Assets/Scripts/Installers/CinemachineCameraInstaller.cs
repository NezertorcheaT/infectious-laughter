using Cinemachine;
using Inventory;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Cinemachine Camera")]
    public class CinemachineCameraInstaller : MonoInstaller
    {
        [SerializeField] private CinemachineVirtualCamera playerCinemachineCamera;
        public override void InstallBindings()
        {
            Container.Bind<CinemachineVirtualCamera>().FromInstance(playerCinemachineCamera).AsSingle().NonLazy();
        }
    }
}