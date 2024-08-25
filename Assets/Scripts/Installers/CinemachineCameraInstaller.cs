using System;
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
        [Inject] private CameraBoundsComposer _boundsComposer;
        private CinemachineConfiner2D confiner;

        public override void InstallBindings()
        {
            var cam = Container.InstantiatePrefab(playerCameraPrefab).GetComponent<PlayerCamera>();
            if (_boundsComposer is not null)
            {
                confiner = cam.VirtualCamera.GetComponent<CinemachineConfiner2D>();
                confiner.m_BoundingShape2D = _boundsComposer.Collider;
                _boundsComposer.OnAdded += Confine;
            }

            Container.Bind<PlayerCamera>().FromInstance(cam).AsSingle().NonLazy();
        }

        private void Confine()
        {
            confiner.InvalidateCache();
        }

        private void OnDestroy()
        {
            _boundsComposer.OnAdded -= Confine;
        }
    }
}