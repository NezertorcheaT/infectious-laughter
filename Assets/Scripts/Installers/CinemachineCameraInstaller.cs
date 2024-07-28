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
        [SerializeField] private CompositeCollider2D bounds;

        public override void InstallBindings()
        {
            var cam = Container.InstantiatePrefab(playerCameraPrefab).GetComponent<PlayerCamera>();
            if (bounds)
                cam.VirtualCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = bounds;

            Container.Bind<PlayerCamera>().FromInstance(cam).AsSingle().NonLazy();
        }
    }
}