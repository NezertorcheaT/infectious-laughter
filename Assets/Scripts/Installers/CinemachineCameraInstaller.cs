using Cinemachine;
using GameFlow;
using UnityEngine;
using Zenject;

namespace Installers
{
    [AddComponentMenu("Installers/Camera")]
    public class CinemachineCameraInstaller : MonoInstaller
    {
        [SerializeField] private PlayerCamera playerCameraPrefab;

        public override void InstallBindings()
        {
            var cam = Container.InstantiatePrefab(playerCameraPrefab).GetComponent<PlayerCamera>();
            Container.Bind<PlayerCamera>().FromInstance(cam).AsSingle().NonLazy();
        }
    }
}