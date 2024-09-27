using CustomHelper;
using UnityEngine;

namespace GameFlow
{
    public class PixelPerfect : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float pixelSize = 1f / 16f;

        private void FixedUpdate()
        {
            transform.position = (transform.position / pixelSize).Round() * pixelSize;
        }
    }
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static Vector3 Round(this Vector3 a) => new(
            Mathf.Round(a.x),
            Mathf.Round(a.y),
            Mathf.Round(a.z)
        );

        public static Vector2 Round(this Vector2 a) => new(
            Mathf.Round(a.x),
            Mathf.Round(a.y)
        );
    }
}