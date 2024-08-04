using CustomHelper;
using UnityEngine;

namespace Levels.Generation
{
    public class PreSpawnedOnFloor : MonoBehaviour
    {
        [field: SerializeField] public Vector2 Center { get; private set; }
        [field: SerializeField, Min(0)] public float Size { get; private set; }
        [field: SerializeField, Min(1)] public int MaxSearchTry { get; private set; } = 100;
        [field: SerializeField, Min(0.01f)] public float SearchPercentRadius { get; private set; } = 0.01f;

        [SerializeField, Min(0)] private float visualSizeY;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(
                Center.ToVector3() + transform.position + new Vector3(0, -visualSizeY / 2f),
                new Vector3(Size, visualSizeY)
            );
            Gizmos.DrawWireCube(
                Center.ToVector3() + transform.position + new Vector3(0, -visualSizeY / 2f),
                new Vector3(Size + MaxSearchTry * SearchPercentRadius * Size, visualSizeY)
            );
        }
    }
}