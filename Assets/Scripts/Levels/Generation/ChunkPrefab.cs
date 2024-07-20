using CustomHelper;
using UnityEngine;

namespace Levels.Generation
{
    public class ChunkPrefab : GenerationPrefab
    {
        [field: SerializeField] public Vector2Int StartPort { get; private set; }
        [field: SerializeField] public Vector2Int EndPort { get; private set; }
        public Grid Grid => grid;
        
        [SerializeField] private Grid grid;

        private void OnDrawGizmosSelected()
        {
            grid ??= GetComponentInChildren<Grid>();
            Gizmos.DrawSphere(Grid.CellToWorld(StartPort.ToVector3Int()), 0.2f);
            Gizmos.DrawSphere(Grid.CellToWorld(EndPort.ToVector3Int()), 0.2f);
        }
    }
}