using Cinemachine;
using UnityEngine;
using Zenject;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Player Get Camera Ability")]
    public class PlayerGetCamera : Ability
    {
        [Inject] private CinemachineVirtualCamera _camera;

        public CinemachineVirtualCamera Camera => _camera;
    }
}