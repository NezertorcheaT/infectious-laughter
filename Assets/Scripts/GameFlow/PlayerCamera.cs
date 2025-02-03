using Cinemachine;
using CustomHelper;
using UnityEngine;
using UnityEngine.UI;

namespace GameFlow
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;
        [SerializeField] private RectTransform canvas;

        [field: SerializeField] public Transform PointTargetForGarbageAnimation { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera VirtualCamera { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public Camera PixelCamera { get; private set; }
        [field: SerializeField] public Camera Camera3D { get; private set; }
        [field: SerializeField] public CinemachineBrain Brain { get; private set; }
        public Texture TextureRender3D => _texture;
        private RenderTexture _texture;
        private Vector2Int _resolution = new(Screen.width, Screen.height);
        private float _orthoSize;

        private void Start()
        {
            _orthoSize = VirtualCamera.m_Lens.OrthographicSize;
            Camera3D.orthographicSize = _orthoSize;
            PixelCamera.orthographicSize = _orthoSize;
            var r = MainCamera.ScreenToWorldPoint(_resolution.ToVector3()) -
                    MainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            canvas.sizeDelta = new Vector2(r.x, r.y);
            canvas.anchoredPosition3D = new Vector3(
                canvas.anchoredPosition3D.x,
                canvas.anchoredPosition3D.y,
                MainCamera.transform.position.z != 0 ? -MainCamera.transform.position.z : 10
            );
            r *= 16f;
            _texture = new RenderTexture((int)r.x, (int)r.y, 24);
            _texture.enableRandomWrite = true;
            _texture.filterMode = FilterMode.Point;
            Camera3D.targetTexture = _texture;
            rawImage.texture = _texture;
            rawImage.color = new Color(1, 1, 1, 1);
        }

        private void Update()
        {
            if (
                _resolution.x == Screen.width &&
                _resolution.y == Screen.height &&
                _orthoSize == VirtualCamera.m_Lens.OrthographicSize
            ) return;
            Debug.Log("Recalculated resolution");
            _resolution = new Vector2Int(Screen.width, Screen.height);
            Start();
        }
    }
}