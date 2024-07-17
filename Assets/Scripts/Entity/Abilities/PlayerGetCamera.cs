using Cinemachine;
using Installers;
using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/PlayerGetCamera Ability")]
    public class PlayerGetCamera : Ability
    {
        [Inject] private PlayerInstallation _playerInstallation;

        public CinemachineVirtualCamera Camera => _playerInstallation.ViewCamera;
    }
}