using GameFlow;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class Canvas3DRenderer : MonoBehaviour
    {
        [SerializeField] private RawImage image;
        [Inject] private PlayerCamera _camera;
        private RenderTexture _texture;

        private void Start()
        {
            _texture = new RenderTexture(
                (int)(Screen.width * 144.0 / 1080.0 / 5 * _camera.MainCamera.orthographicSize),
                (int)(Screen.height * 144.0 / 1080.0 / 5 * _camera.MainCamera.orthographicSize),
                24
            );
            _texture.enableRandomWrite = true;
            _texture.filterMode = FilterMode.Point;
            _camera.Camera3D.targetTexture = _texture;
            image.texture = _texture;
            image.color = new Color(1, 1, 1, 1);
        }
    }
}