using Cinemachine;
using GameFlow;
using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Camera to Items Provider")]
    public class CameraToItemsProvider : Ability
    {
        [Inject] private PlayerCamera _camera;

        public CinemachineVirtualCamera Camera => _camera.VirtualCamera;
    }
}