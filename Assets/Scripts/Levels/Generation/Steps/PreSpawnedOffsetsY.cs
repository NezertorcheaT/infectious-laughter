using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Offset by Y")]
    public class PreSpawnedOffsetsY : GenerationStep
    {
        public override void Execute(LevelGeneration.Properties levelGeneration)
        {
            for (var i = 0; i < levelGeneration.PreSpawns.Count; i++)
            {
                var hitDown = Physics2D.Raycast(
                    new Vector2(levelGeneration.PreSpawns[i].Position.x, levelGeneration.MaxY),
                    Vector2.down,
                    levelGeneration.MaxY * 5,
                    1 << 0
                );
                if (!hitDown.collider) continue;
                levelGeneration.PreSpawns[i] = new LevelGeneration.Properties.PreSpawned
                {
                    Prefab = levelGeneration.PreSpawns[i].Prefab,
                    Position = levelGeneration.PreSpawns[i].Position,
                    Rotation = levelGeneration.PreSpawns[i].Rotation,
                    OffsetY = levelGeneration.PreSpawns[i].Position.y > hitDown.point.y
                        ? -(Mathf.Abs(levelGeneration.PreSpawns[i].Position.y) - Mathf.Abs(hitDown.point.y))
                        : (Mathf.Abs(hitDown.point.y) - Mathf.Abs(levelGeneration.PreSpawns[i].Position.y))
                };
            }
        }
    }
}