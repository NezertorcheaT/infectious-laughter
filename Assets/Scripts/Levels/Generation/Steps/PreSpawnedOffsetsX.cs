using System;
using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Offset by X")]
    public class PreSpawnedOffsetsX : GenerationStep
    {
        public override void Execute(LevelGeneration.Properties levelGeneration)
        {
            for (var i = 0; i < levelGeneration.PreSpawns.Count; i++)
            {
                var preSpawned = levelGeneration.PreSpawns[i];
                var onFloor = preSpawned.Prefab.GetComponent<PreSpawnedOnFloor>();
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
                        new Vector2(preSpawned.Position.x + onFloor.Center.x + offsetX, levelGeneration.MaxY),
                        Vector2.down,
                        levelGeneration.MaxY * 5,
                        1 << 0
                    );
                    var hitLeft = Physics2D.Raycast(
                        new Vector2(preSpawned.Position.x + onFloor.Center.x - onFloor.Size / 2f + offsetX,
                            levelGeneration.MaxY),
                        Vector2.down,
                        levelGeneration.MaxY * 5,
                        1 << 0
                    );
                    var hitRight = Physics2D.Raycast(
                        new Vector2(preSpawned.Position.x + onFloor.Center.x + onFloor.Size / 2f + offsetX,
                            levelGeneration.MaxY),
                        Vector2.down,
                        levelGeneration.MaxY * 5,
                        1 << 0
                    );
                    if (!hitLeft.collider && !hitRight.collider && !hitMiddle.collider) continue;
                    if (Math.Abs(hitLeft.point.y - hitRight.point.y) > searchTickRange) continue;
                    if (Math.Abs(hitMiddle.point.y - hitRight.point.y) > searchTickRange) continue;

                    offset = offsetX;
                    break;
                }

                levelGeneration.PreSpawns[i] = new LevelGeneration.Properties.PreSpawned
                {
                    Prefab = preSpawned.Prefab,
                    Position = preSpawned.Position,
                    Rotation = preSpawned.Rotation,
                    OffsetY = preSpawned.OffsetY,
                    OffsetX = offset,
                };
            }
        }
    }
}