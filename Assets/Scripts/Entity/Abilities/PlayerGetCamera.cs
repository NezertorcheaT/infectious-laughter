using Cinemachine;
using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/PlayerGetCamera Ability")]
    public class PlayerGetCamera : Ability
    {
        [Inject] private CinemachineVirtualCamera _camera;

        public CinemachineVirtualCamera Camera => _camera;
    }
}