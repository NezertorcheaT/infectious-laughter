using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Fill Empty With")]
    public class FillEmptyWith : TilemapStep
    {
        [Tooltip("этот тайл будет заполнять карту полностью")] [SerializeField]
        private TileBase tile;

        public override void Execute(LevelGeneration.Properties levelGeneration)
        {
            foreach (var position in levelGeneration.Tilemap.cellBounds.allPositionsWithin)
            {
                if (!levelGeneration.Tilemap.GetTile(position))
                    levelGeneration.Tilemap.SetTile(position, tile);
            }
        }
    }
}