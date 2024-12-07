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
            var r = _camera.MainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)) -
                    _camera.MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            r *= 16f;
            _texture = new RenderTexture(
                (int)Mathf.Abs(r.x),
                (int)Mathf.Abs(r.y),
                24
            );
            _texture.enableRandomWrite = true;
            _texture.filterMode = FilterMode.Point;
            _camera.Camera3D.targetTexture = _texture;
            image.texture = _texture;
            image.color = new Color(1, 1, 1, 1);
        }

        private static int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }
    }
}