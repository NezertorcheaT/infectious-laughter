using GameFlow;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class TrashTrail : MonoBehaviour
    {
        [SerializeField] private RawImage image;
        [Inject] private PlayerCamera _camera;
        private RenderTexture _texture;

        private void Start()
        {
            _texture = new RenderTexture((int)(Screen.width / 16f * 2.5f), (int)(Screen.height / 16f * 2.5f), 24);
            _texture.enableRandomWrite = true;
            _texture.filterMode = FilterMode.Point;
            _camera.PixelCamera.targetTexture = _texture;
            image.texture = _texture;
            image.color = new Color(1, 1, 1, 1);
        }
    }
}