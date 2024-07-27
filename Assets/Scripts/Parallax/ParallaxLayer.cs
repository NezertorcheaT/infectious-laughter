using GameFlow;
using UnityEngine;
using Zenject;

namespace Parallax
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private Vector2 parralaxMultiplier;
        [Inject] private PlayerCamera _camera;

        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;

        private void Start()
        {
            _cameraTransform = _camera.MainCamera.transform;
            _lastCameraPosition = _camera.MainCamera.transform.position;
        }

        private void LateUpdate()
        {
            var deltaMovement = _cameraTransform.position - _lastCameraPosition;

            gameObject.transform.position += new Vector3(
                deltaMovement.x * parralaxMultiplier.x,
                deltaMovement.y * parralaxMultiplier.y,
                0f
            );
            _lastCameraPosition = _cameraTransform.position;
        }
    }
}