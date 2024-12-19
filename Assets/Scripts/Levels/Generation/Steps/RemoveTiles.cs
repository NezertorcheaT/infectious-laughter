using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Remove tiles")]
    public class RemoveTiles : TilemapStep
    {
        [Tooltip("этот тайл будет удалён с карты полностью")]
        [SerializeField] private TileBase tileToRemove;

        public override void Execute(LevelGeneration.Properties levelGeneration)
        {
            foreach (var position in levelGeneration.Tilemap.cellBounds.allPositionsWithin)
            {
                if (levelGeneration.Tilemap.GetTile(position) == tileToRemove)
                    levelGeneration.Tilemap.SetTile(position, null);
            }
        }
    }
}