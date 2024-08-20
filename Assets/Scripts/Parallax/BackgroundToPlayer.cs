using GameFlow;
using UnityEngine;
using Zenject;

namespace Parallax
{
    public class BackgroundToPlayer : MonoBehaviour
    {
        [Inject] private PlayerCamera _playerCamera;

        private void Start()
        {
            var position = _playerCamera.MainCamera.transform.position;
            position.z = 0;
            transform.position = position;
        }
    }
}