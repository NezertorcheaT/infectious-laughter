using CustomHelper;
using UnityEngine;

namespace Levels.Generation
{
    public class StructurePrefab : GenerationPrefab
    {
        [field: SerializeField] public Vector2Int MinPosition { get; private set; }
        [field: SerializeField] public Vector2Int MaxPosition { get; private set; }
        
        [field: Tooltip("уровень земли")]
        [field: SerializeField] public int Ground { get; private set; }
        [field: SerializeField] public float IntersectingRemoveExpand { get; private set; } = 1.5f;
        public Grid Grid => grid;

        public Bounds WorldBounds
        {
            get
            {
                var scale = Grid.CellToWorld(MaxPosition.ToVector3Int()) - Grid.CellToWorld(MinPosition.ToVector3Int());
                return new Bounds(
                    Grid.CellToWorld(MaxPosition.ToVector3Int()) - scale / 2f,
                    scale
                );
            }
        }

        public BoundsInt CellBounds
        {
            get
            {
                var scale = MaxPosition - MinPosition;
                return new BoundsInt(MinPosition.x, MinPosition.y, 0, scale.x, scale.y, 0);
            }
        }

        [SerializeField] private Grid grid;

        private void OnDrawGizmosSelected()
        {
            grid ??= GetComponentInChildren<Grid>();
            Gizmos.DrawSphere(Grid.CellToWorld(MinPosition.ToVector3Int()), 0.2f);
            Gizmos.DrawSphere(Grid.CellToWorld(MaxPosition.ToVector3Int()), 0.2f);
            Gizmos.DrawLine(
                Grid.CellToWorld(new Vector3Int(MinPosition.x - 1, Ground)),
                Grid.CellToWorld(new Vector3Int(MaxPosition.x + 1, Ground))
            );
            var expanded = WorldBounds;
            expanded.Expand(IntersectingRemoveExpand);
            Gizmos.DrawWireCube(WorldBounds.center, WorldBounds.size);
            Gizmos.color = new Color(1f, 0.5f, 0.5f, 1f);
            Gizmos.DrawWireCube(expanded.center, expanded.size);
        }
    }
}