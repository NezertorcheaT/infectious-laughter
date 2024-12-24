using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Cleaning")]
    public class CleanChunks : TilemapStep
    {
        public override void Execute(LevelGeneration.Properties properties)
        {
            foreach (var position in properties.Tilemap.cellBounds.allPositionsWithin)
            {
                if (properties.Tilemap.GetTile(position) is not null && (
                        (
                            properties.Tilemap.GetTile(position + new Vector3Int(1, 0)) is null &&
                            properties.Tilemap.GetTile(position + new Vector3Int(0, 1)) is null &&
                            properties.Tilemap.GetTile(position + new Vector3Int(-1, 0)) is null
                        ) || (
                            properties.Tilemap.GetTile(position + new Vector3Int(1, 0)) is null &&
                            properties.Tilemap.GetTile(position + new Vector3Int(0, 1)) is null &&
                            properties.Tilemap.GetTile(position + new Vector3Int(0, -1)) is null
                        ) || (
                            properties.Tilemap.GetTile(position + new Vector3Int(-1, 0)) is null &&
                            properties.Tilemap.GetTile(position + new Vector3Int(0, 1)) is null &&
                            properties.Tilemap.GetTile(position + new Vector3Int(0, -1)) is null
                        )
                    )
                )
                {
                    properties.Tilemap.SetTile(position, null);
                }
            }
        }
    }
}