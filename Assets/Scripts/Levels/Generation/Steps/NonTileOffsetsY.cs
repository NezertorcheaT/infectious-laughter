using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Offset by Y")]
    public class NonTileOffsetsY : GenerationStep
    {
        public override void Execute(LevelGeneration.Properties properties)
        {
            for (var i = 0; i < properties.NonTileObjects.Count; i++)
            {
                var hitDown = Physics2D.Raycast(
                    new Vector2(properties.NonTileObjects[i].Position.x, properties.MaxY),
                    Vector2.down,
                    properties.MaxY * 5,
                    1 << 0
                );
                if (!hitDown.collider) continue;
                properties.NonTileObjects[i] = new LevelGeneration.Properties.NonTileObject
                {
                    Prefab = properties.NonTileObjects[i].Prefab,
                    Position = properties.NonTileObjects[i].Position,
                    Rotation = properties.NonTileObjects[i].Rotation,
                    OffsetY = properties.NonTileObjects[i].Position.y > hitDown.point.y
                        ? -(Mathf.Abs(properties.NonTileObjects[i].Position.y) - Mathf.Abs(hitDown.point.y))
                        : (Mathf.Abs(hitDown.point.y) - Mathf.Abs(properties.NonTileObjects[i].Position.y))
                };
            }
        }
    }
}