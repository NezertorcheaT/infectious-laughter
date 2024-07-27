using GameFlow;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Parallax
{
    public class ParallaxLayer : MonoBehaviour
    {
        [FormerlySerializedAs("parralaxMultiplier")] [SerializeField] private Vector2 parallaxMultiplier;
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
                deltaMovement.x * parallaxMultiplier.x,
                deltaMovement.y * parallaxMultiplier.y,
                0f
            );
            _lastCameraPosition = _cameraTransform.position;
        }
    }
}