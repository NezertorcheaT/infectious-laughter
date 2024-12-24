using System;
using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Offset by X")]
    public class NonTileOffsetsX : GenerationStep
    {
        public override void Execute(LevelGeneration.Properties properties)
        {
            for (var i = 0; i < properties.NonTileObjects.Count; i++)
            {
                var nonTile = properties.NonTileObjects[i];
                var onFloor = nonTile.Prefab.GetComponent<PreSpawnedOnFloor>();
                if (onFloor is null) continue;

                var offset = 0f;
                var searchDirection = false;
                var searchTickRange = onFloor.SearchUnit;
                for (var j = 0; j < onFloor.MaxSearchTry; j++)
                {
                    if (searchDirection) j -= 1;
                    searchDirection = !searchDirection;
                    var searchDirectionMultiplier = searchDirection ? 1f : -1f;
                    var offsetX = searchDirectionMultiplier * searchTickRange * j;

                    var hitMiddle = Physics2D.Raycast(
                        new Vector2(nonTile.Position.x + onFloor.Center.x + offsetX, properties.MaxY),
                        Vector2.down,
                        properties.MaxY * 5,
                        1 << 0
                    );
                    var hitLeft = Physics2D.Raycast(
                        new Vector2(nonTile.Position.x + onFloor.Center.x - onFloor.Size / 2f + offsetX,
                            properties.MaxY),
                        Vector2.down,
                        properties.MaxY * 5,
                        1 << 0
                    );
                    var hitRight = Physics2D.Raycast(
                        new Vector2(nonTile.Position.x + onFloor.Center.x + onFloor.Size / 2f + offsetX,
                            properties.MaxY),
                        Vector2.down,
                        properties.MaxY * 5,
                        1 << 0
                    );
                    if (!hitLeft.collider && !hitRight.collider && !hitMiddle.collider) continue;
                    if (Math.Abs(hitLeft.point.y - hitRight.point.y) > searchTickRange) continue;
                    if (Math.Abs(hitMiddle.point.y - hitRight.point.y) > searchTickRange) continue;

                    offset = offsetX;
                    break;
                }

                properties.NonTileObjects[i] = new LevelGeneration.Properties.NonTileObject
                {
                    Prefab = nonTile.Prefab,
                    Position = nonTile.Position,
                    Rotation = nonTile.Rotation,
                    OffsetY = nonTile.OffsetY,
                    OffsetX = offset,
                };
            }
        }
    }
}