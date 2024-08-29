using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Cleaning")]
    public class CleanChunks : TilemapStep
    {
        public override void Execute(LevelGeneration.Properties levelGeneration)
        {
            foreach (var position in levelGeneration.Tilemap.cellBounds.allPositionsWithin)
            {
                if (levelGeneration.Tilemap.GetTile(position) is not null && (
                        (
                            levelGeneration.Tilemap.GetTile(position + new Vector3Int(1, 0)) is null &&
                            levelGeneration.Tilemap.GetTile(position + new Vector3Int(0, 1)) is null &&
                            levelGeneration.Tilemap.GetTile(position + new Vector3Int(-1, 0)) is null
                        ) || (
                            levelGeneration.Tilemap.GetTile(position + new Vector3Int(1, 0)) is null &&
                            levelGeneration.Tilemap.GetTile(position + new Vector3Int(0, 1)) is null &&
                            levelGeneration.Tilemap.GetTile(position + new Vector3Int(0, -1)) is null
                        ) || (
                            levelGeneration.Tilemap.GetTile(position + new Vector3Int(-1, 0)) is null &&
                            levelGeneration.Tilemap.GetTile(position + new Vector3Int(0, 1)) is null &&
                            levelGeneration.Tilemap.GetTile(position + new Vector3Int(0, -1)) is null
                        )
                    )
                )
                {
                    levelGeneration.Tilemap.SetTile(position, null);
                }
            }
        }
    }
}