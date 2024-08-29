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
        private Structure[] structures;

        [Tooltip("Максимальное колличество попыток впихнуть структуру в мир, перед тем как отвергнуть её")]
        [SerializeField, Min(1)]
        private int structuresSpawnMaxTry = 100;

        private List<BoundsInt> _filledWithStructure;
        private LevelGeneration.Properties _levelGeneration;

        [Serializable]
        private struct Structure
        {
            public StructurePrefab structure;

            [Tooltip("колличество структур")] [Min(1)]
            public int count;
        }

        public override void Execute(LevelGeneration.Properties levelGeneration)
        {
            _levelGeneration = levelGeneration;
            _filledWithStructure = new List<BoundsInt>();
            foreach (var structurePrefab in structures)
            {
                if (structurePrefab.structure is null)
                    continue;

                for (var i = 0; i < Mathf.Max(structurePrefab.count, 1); i++)
                {
                    var pos = levelGeneration.Tilemap.GridRay(
                        new Vector2Int(
                            levelGeneration.Random.Next(levelGeneration.StructureMinX, levelGeneration.StructureMaxX),
                            levelGeneration.MaxY * 2),
                        Vector2.down);
                    var spawnTry = 0;

                    while (
                        spawnTry < structuresSpawnMaxTry &&
                        (pos is null || !CheckStructureAtPos(structurePrefab.structure, pos.Value, levelGeneration))
                    )
                    {
                        spawnTry += 1;
                        pos = levelGeneration.Tilemap.GridRay(
                            new Vector2Int(
                                levelGeneration.Random.Next(levelGeneration.StructureMinX,
                                    levelGeneration.StructureMaxX),
                                levelGeneration.MaxY),
                            Vector2.down);
                    }

                    if (spawnTry >= structuresSpawnMaxTry - 1)
                        continue;

                    AddStructureAtPos(structurePrefab.structure,
                        pos.Value + new Vector2Int(0, 1 - structurePrefab.structure.Ground), levelGeneration);
                }
            }
        }

        private bool CheckStructureAtPos(StructurePrefab structure, Vector2Int gridPosition,
            LevelGeneration.Properties levelGeneration)
        {
            var structureBounds = structure.CellBounds;
            structureBounds.position += gridPosition.ToVector3Int();

            if (structureBounds.xMin < levelGeneration.StructureMinX)
                return false;
            if (structureBounds.xMax > levelGeneration.StructureMaxX)
                return false;
            if (_filledWithStructure.Count != 0 && _filledWithStructure.Any(i => i.Intersects2D(structureBounds)))
                return false;

            return true;
        }

        private void AddStructureAtPos(StructurePrefab structure, Vector2Int gridPosition,
            LevelGeneration.Properties levelGeneration)
        {
            var worldBounds = structure.WorldBounds;
            worldBounds.center += levelGeneration.Tilemap.layoutGrid.CellToWorld(gridPosition.ToVector3Int());
            worldBounds.Expand(structure.IntersectingRemoveExpand);

            var cb = structure.CellBounds;
            cb.position += gridPosition.ToVector3Int();
            _filledWithStructure.Add(cb);

            var intersectingPreSpawns = levelGeneration.PreSpawns
                    .Where(i => worldBounds.Contains2D(i.Position + new Vector3(0, i.OffsetY)))
                    .ToArray()
                ;
            foreach (var toRemove in intersectingPreSpawns)
            {
                levelGeneration.PreSpawns.Remove(toRemove);
            }

            foreach (var noneGrid in structure.NoneGridObjects)
            {
                levelGeneration.PreSpawns.Add(new LevelGeneration.Properties.PreSpawned
                {
                    Prefab = noneGrid.gameObject,
                    Position =
                        levelGeneration.Tilemap.layoutGrid.CellToWorld(gridPosition.ToVector3Int()) +
                        noneGrid.transform.localPosition,
                    Rotation = noneGrid.gameObject.transform.rotation,
                    OffsetY = 0
                });
            }

            levelGeneration.Tilemap.Insert(structure.Tilemap, gridPosition, levelGeneration.VoidTile);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_filledWithStructure is null) return;
            foreach (var bounds in _filledWithStructure)
            {
                Gizmos.DrawWireCube(
                    _levelGeneration.Tilemap.layoutGrid.CellToWorld(bounds.position) +
                    _levelGeneration.Tilemap.layoutGrid.CellToWorld(bounds.size) / 2f,
                    _levelGeneration.Tilemap.layoutGrid.CellToWorld(bounds.size));
            }
        }
#endif
    }
}