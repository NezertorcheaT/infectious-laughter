using CustomHelper;
using UnityEngine;

namespace Levels.Generation
{
    public class PreSpawnedOnFloor : MonoBehaviour
    {
        [field: Tooltip("центер области пола")]
        [field: SerializeField] public Vector2 Center { get; private set; }
        
        [field: Tooltip("ширина области пола")]
        [field: SerializeField, Min(0)] public float Size { get; private set; }
        
        [field: Tooltip("колличество попыток поиска")]
        [field: SerializeField, Min(1)] public int MaxSearchTry { get; private set; } = 100;
        
        [field: Tooltip("процент ширины (0-1), служащий минимальной единицей поиска")]
        [field: SerializeField, Min(0.01f)] public float SearchPercentRadius { get; private set; } = 0.01f;
        
        [Tooltip("размер коробок вниз, не влияет на конечный результат")]
        [SerializeField, Min(0)] private float visualSizeY = 4f;

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