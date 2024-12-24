using System;
using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Structures")]
    public class StructuresGeneration : TilemapStep
    {
        [Tooltip("Это структуры для спавна")] [SerializeField]
        private StructureRepresentation[] structures;

        [Tooltip("Максимальное колличество попыток впихнуть структуру в мир, перед тем как отвергнуть её")]
        [SerializeField, Min(1)]
        private int structuresSpawnMaxTry = 100;

        private List<BoundsInt> _filledWithStructure;
        private LevelGeneration.Properties _properties;

        [Serializable]
        private struct StructureRepresentation
        {
            public StructurePrefab structure;

            [Tooltip("колличество структур")] [Min(1)]
            public int count;
        }

        public override void Execute(LevelGeneration.Properties properties)
        {
            _properties = properties;
            _filledWithStructure = new List<BoundsInt>();
            foreach (var structurePrefab in structures)
            {
                if (structurePrefab.structure is null) continue;

                for (var i = 0; i < Mathf.Max(structurePrefab.count, 1); i++)
                {
                    var hit = properties.Tilemap.GridRay(
                        new Vector2Int(
                            properties.Random.Next(properties.StructureMinX, properties.StructureMaxX),
                            properties.MaxY * 2),
                        Vector2.down);
                    var spawnTry = 0;

                    while (
                        spawnTry < structuresSpawnMaxTry &&
                        (!hit.isHit || !CheckStructureAtPos(structurePrefab.structure, hit.point, properties))
                    )
                    {
                        spawnTry++;
                        hit = properties.Tilemap.GridRay(
                            new Vector2Int(
                                properties.Random.Next(properties.StructureMinX,
                                    properties.StructureMaxX),
                                properties.MaxY),
                            Vector2.down);
                    }

                    if (spawnTry >= structuresSpawnMaxTry - 1)
                        continue;

                    AddStructureAtPos(
                        structurePrefab.structure,
                        hit.point + new Vector2Int(0, 1 - structurePrefab.structure.Ground),
                        properties
                    );
                }
            }
        }

        private bool CheckStructureAtPos(StructurePrefab structure, Vector2Int gridPosition,
            LevelGeneration.Properties properties)
        {
            var structureBounds = structure.CellBounds;
            structureBounds.position += gridPosition.ToVector3Int();

            if (structureBounds.xMin < properties.StructureMinX)
                return false;
            if (structureBounds.xMax > properties.StructureMaxX)
                return false;
            if (_filledWithStructure.Count != 0 && _filledWithStructure.Any(i => i.Intersects2D(structureBounds)))
                return false;

            var width = structure.MaxPosition.x - structure.MinPosition.x;
            var heights = new int[width];
            for (var i = 0; i < width; i++)
            {
                var hit = properties.Tilemap.GridRay(
                    new Vector2Int(gridPosition.x + structure.MinPosition.x + i, properties.MaxY * 2),
                    Vector2.down
                );
                if (!hit.isHit) return false;
                heights[i] = hit.distance;
            }

            var max = heights.Max();
            for (var i = 0; i < width; i++)
                heights[i] = Mathf.Abs(heights[i] - max);

            return heights.Max() <= structure.Flattines;
        }

        private void AddStructureAtPos(StructurePrefab structure, Vector2Int gridPosition,
            LevelGeneration.Properties properties)
        {
            var worldBounds = structure.WorldBounds;
            worldBounds.center += properties.Tilemap.layoutGrid.CellToWorld(gridPosition.ToVector3Int());
            worldBounds.Expand(structure.IntersectingRemoveExpand);

            var cb = structure.CellBounds;
            cb.position += gridPosition.ToVector3Int();
            _filledWithStructure.Add(cb);

            foreach (var toRemove in properties.NonTileObjects
                         .Where(i => worldBounds.Contains2D(i.Position) &&
                                     !i.Prefab.TryGetComponent(out PreSpawnedPersistent _))
                         .ToArray())
                properties.NonTileObjects.Remove(toRemove);

            foreach (var noneGrid in structure.NoneTileChildren)
            {
                properties.NonTileObjects.Add(new LevelGeneration.Properties.NonTileObject
                {
                    Prefab = noneGrid.gameObject,
                    Position =
                        properties.Tilemap.layoutGrid.CellToWorld(gridPosition.ToVector3Int()) +
                        noneGrid.transform.localPosition,
                    Rotation = noneGrid.gameObject.transform.rotation,
                    OffsetY = 0
                });
            }

            properties.Tilemap.Insert(structure.Tilemap, gridPosition);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_filledWithStructure is null) return;
            foreach (var bounds in _filledWithStructure)
            {
                Gizmos.DrawWireCube(
                    _properties.Tilemap.layoutGrid.CellToWorld(bounds.position) +
                    _properties.Tilemap.layoutGrid.CellToWorld(bounds.size) / 2f,
                    _properties.Tilemap.layoutGrid.CellToWorld(bounds.size));
            }
        }
#endif
    }
}