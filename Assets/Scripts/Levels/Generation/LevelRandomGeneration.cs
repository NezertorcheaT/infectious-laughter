using System;
using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Levels.Generation
{
    public class LevelRandomGeneration : MonoBehaviour
    {
        [Header("База")] [SerializeField] public string seed;
        [SerializeField] private TileBase voidTile;
        [SerializeField] private Tilemap tilemap;

        [Header("Чанки")] [Tooltip("Это кароч колличество основных чанков, без специальных")] [SerializeField, Min(1)]
        private int chunksCount;

        [SerializeField] private ChunkPrefab firstChunk;
        [SerializeField] private ChunkPrefab lastChunk;
        [SerializeField] private ChunkPrefab[] chunkBases;
        [SerializeField] private ChunkPrefab[] specialChunks;

        [Header("Изменяющие слои")] [SerializeField]
        private OffsetLayer[] layers;

        [Header("Структуры")] [SerializeField] private Structure[] structures;
        [SerializeField, Min(1)] private int structuresSpawnMaxTry = 100;

        private struct PreSpawned
        {
            public GameObject Prefab;
            public Vector3 Position;
            public Quaternion Rotation;
        }

        [Serializable]
        private struct Structure
        {
            public StructurePrefab structure;
            [Min(1)] public int count;
        }

        [Serializable]
        private class OffsetLayer
        {
            public enum Behavior
            {
                Clamp,
                Repeat,
                Mirror
            }

            public enum Blending
            {
                Add,
                Subtract
            }

            public GroundOffsetLayer layer;
            public Blending blending;
            public Behavior behavior;
            public int offset;
        }

        private List<PreSpawned> _preSpawned;
        private int _structureMinX;
        private int _structureMaxX;
        private List<BoundsInt> _filledWithStructure;
        private Random _random;
        private int _layerMinX;
        private int _layerMaxX;

        public void StartGeneration()
        {
            _random = new Random(seed.GetHashCode());
            GenerateChunks();
            ApplyLayers();
            GenerateStructures();
        }


        private void ProcessLayer(IEnumerable<float> map, OffsetLayer layer, int from, int to)
        {
            map = map.Select(i => i * (layer.blending is OffsetLayer.Blending.Subtract ? -1f : 1f));
            var enumerator = map.GetEnumerator();

            try
            {
                for (var i = 0; i < layer.offset; i++)
                {
                    enumerator.MoveNext();
                }

                var x = from;
                while (enumerator.MoveNext())
                {
                    if (x >= to) break;
                    var current = enumerator.Current;
                    if (current == 0) continue;

                    var ray = GridRay(new Vector2Int(x, 50), Vector2.down);
                    if (ray is null) continue;

                    if (current > 0)
                    {
                        for (var i = 0; i < current+1; i++)
                        {
                            tilemap.SetTile(new Vector3Int(x, ray.Value.y + i), layer.layer.Tile);
                        }
                    }
                    else
                    {
                        tilemap.SetTile(new Vector3Int(x, ray.Value.y), null);
                        for (var i = 0; i < -current+1; i++)
                        {
                            tilemap.SetTile(new Vector3Int(x, ray.Value.y - i + 1), null);
                        }
                    }

                    x++;
                }
            }
            finally
            {
                enumerator?.Dispose();
            }
        }

        private void ApplyLayers()
        {
            foreach (var offsetLayer in layers)
            {
                if (offsetLayer.layer.Infinite)
                {
                    ProcessLayer(offsetLayer.layer.GetMap(seed), offsetLayer, _layerMinX, _layerMaxX);
                    return;
                }

                var map = offsetLayer.layer.GetMap(seed).ToArray();
                offsetLayer.offset = (int) Mathf.Repeat(offsetLayer.offset, map.Length);
                if (offsetLayer.behavior is OffsetLayer.Behavior.Clamp)
                    ProcessLayer(map, offsetLayer, _layerMinX, Mathf.Clamp(map.Length - 1, _layerMinX, _layerMaxX));
                else if (offsetLayer.behavior is OffsetLayer.Behavior.Repeat)
                {
                    for (var i = -1; i < _layerMaxX / map.Length; i++)
                    {
                        ProcessLayer(map, offsetLayer,
                            Mathf.Clamp((map.Length - 1) * i, _layerMinX, _layerMaxX),
                            Mathf.Clamp((map.Length - 1) * (i + 1), _layerMinX, _layerMaxX)
                        );
                    }
                }
                else if (offsetLayer.behavior is OffsetLayer.Behavior.Mirror)
                {
                    for (var i = -1; i < _layerMaxX / map.Length; i++)
                    {
                        map = map.Reverse().ToArray();
                        ProcessLayer(map, offsetLayer,
                            Mathf.Clamp((map.Length - 1) * i, _layerMinX, _layerMaxX),
                            Mathf.Clamp((map.Length - 1) * (i + 1), _layerMinX, _layerMaxX)
                        );
                    }
                }
            }
        }

        private bool CheckStructureAtPos(StructurePrefab structure, Vector2Int gridPosition)
        {
            var structureBounds = structure.CellBounds;
            structureBounds.position += gridPosition.ToVector3Int();

            if (structureBounds.xMin < _structureMinX)
                return false;
            if (structureBounds.xMax > _structureMaxX)
                return false;
            if (_filledWithStructure.Count != 0 && _filledWithStructure.Any(i => i.Intersects2D(structureBounds)))
                return false;

            return true;
        }

        private void AddStructureAtPos(StructurePrefab structure, Vector2Int gridPosition)
        {
            var worldBounds = structure.WorldBounds;
            worldBounds.center += tilemap.layoutGrid.CellToWorld(gridPosition.ToVector3Int());
            worldBounds.Expand(structure.IntersectingRemoveExpand);

            var cb = structure.CellBounds;
            cb.position += gridPosition.ToVector3Int();
            _filledWithStructure.Add(cb);

            var intersectingPreSpawns = _preSpawned.Where(i => worldBounds.Contains2D(i.Position)).ToArray();
            foreach (var toRemove in intersectingPreSpawns)
            {
                _preSpawned.Remove(toRemove);
            }

            foreach (var noneGrid in structure.NoneGridObjects)
            {
                _preSpawned.Add(new PreSpawned
                {
                    Prefab = noneGrid.gameObject,
                    Position = tilemap.layoutGrid.CellToWorld(gridPosition.ToVector3Int()) +
                               noneGrid.transform.localPosition,
                    Rotation = noneGrid.gameObject.transform.rotation,
                });
            }

            Insert(structure.Tilemap, gridPosition);
        }

        private Vector2Int? GridRay(Vector2Int gridPosition, Vector2 direction)
        {
            BoundsInt cellBounds;
            return GridRay(gridPosition, direction,
                (int) ((cellBounds = tilemap.cellBounds).min - cellBounds.max).magnitude);
        }

        private Vector2Int? GridRay(Vector2Int gridPosition, Vector2 direction, int distance)
        {
            for (var i = 0; i < distance; i++)
            {
                var pos = (gridPosition.ToVector2() + direction * i).ToVector3Int();
                if (tilemap.HasTile(pos))
                    return pos.ToVector2Int();
            }

            return null;
        }

        private void GenerateStructures()
        {
            _filledWithStructure = new List<BoundsInt>();
            foreach (var structurePrefab in structures)
            {
                if (structurePrefab.structure is null)
                    continue;

                for (var i = 0; i < Mathf.Max(structurePrefab.count, 1); i++)
                {
                    var pos = GridRay(new Vector2Int(_random.Next(_structureMinX, _structureMaxX), 20), Vector2.down);
                    var spawnTry = 0;

                    while (
                        spawnTry < structuresSpawnMaxTry &&
                        (pos is null || !CheckStructureAtPos(structurePrefab.structure, pos.Value))
                    )
                    {
                        spawnTry += 1;
                        pos = GridRay(new Vector2Int(_random.Next(_structureMinX, _structureMaxX), 10), Vector2.down);
                    }

                    if (spawnTry >= structuresSpawnMaxTry - 1)
                        continue;

                    AddStructureAtPos(structurePrefab.structure, pos.Value);
                }
            }
        }

        private void GenerateChunks()
        {
            var chunks = SetupChunks().ToArray();
            _preSpawned = new List<PreSpawned>();

            var portOffset = new Vector2Int();
            _structureMinX = (chunks[0].EndPort - chunks[0].StartPort).x;
            foreach (var chunk in chunks)
            {
                foreach (var noneGrid in chunk.NoneGridObjects)
                {
                    _preSpawned.Add(new PreSpawned
                    {
                        Prefab = noneGrid.gameObject,
                        Position = tilemap.layoutGrid.CellToWorld(portOffset.ToVector3Int()) -
                                   chunk.Grid.CellToWorld(chunk.StartPort.ToVector3Int()) +
                                   noneGrid.transform.localPosition,
                        Rotation = Quaternion.identity,
                    });
                }

                Insert(chunk.Tilemap, portOffset - chunk.StartPort);
                portOffset += chunk.EndPort - chunk.StartPort;
                _structureMaxX = portOffset.x - (chunk.EndPort - chunk.StartPort).x;
            }

            _layerMinX = 0;
            _layerMaxX = portOffset.x;
        }

        private IEnumerable<ChunkPrefab> SetupChunks()
        {
            var chunks = new List<ChunkPrefab>();
            if (firstChunk) chunks.Add(firstChunk);

            for (var i = 0; i < chunksCount; i++)
            {
                chunks.Add(chunkBases[_random.Next(0, chunkBases.Length)]);
            }

            foreach (var specialChunk in specialChunks)
            {
                chunks.Insert(_random.Next(1, chunkBases.Length - 1), specialChunk);
            }

            if (lastChunk) chunks.Add(lastChunk);
            return chunks;
        }

        private void Insert(Tilemap map, Vector2Int offset)
        {
            foreach (var mapPos in map.cellBounds.allPositionsWithin)
            {
                if (map.HasTile(mapPos))
                    tilemap.SetTile(mapPos + offset.ToVector3Int(), map.GetTile(mapPos));

                if (map.GetTile(mapPos) == voidTile)
                    tilemap.SetTile(mapPos + offset.ToVector3Int(), null);
            }
        }

        public void InstantiatePreSpawned(Func<GameObject, Vector3, Quaternion, Transform, GameObject> instantiate)
        {
            foreach (var preSpawned in _preSpawned)
            {
                var i = instantiate(preSpawned.Prefab, preSpawned.Position, preSpawned.Rotation, null);
                i.SetActive(true);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_filledWithStructure is null) return;
            foreach (var bounds in _filledWithStructure)
            {
                Gizmos.DrawWireCube(
                    tilemap.layoutGrid.CellToWorld(bounds.position) + tilemap.layoutGrid.CellToWorld(bounds.size) / 2f,
                    tilemap.layoutGrid.CellToWorld(bounds.size));
            }
        }
#endif
    }
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static Vector2Int ToInt(this Vector2 a) => new Vector2Int((int) a.x, (int) a.y);
        public static Vector3Int ToInt(this Vector3 a) => new Vector3Int((int) a.x, (int) a.y, (int) a.z);
        public static Vector3Int ToVector3Int(this Vector2Int a) => new Vector3Int((int) a.x, (int) a.y);
        public static Vector3Int ToVector3Int(this Vector2 a) => new Vector3Int((int) a.x, (int) a.y);
        public static Vector2Int ToVector2Int(this Vector2 a) => new Vector2Int((int) a.x, (int) a.y);
        public static Vector3Int ToVector3Int(this Vector3 a) => new Vector3Int((int) a.x, (int) a.y);
        public static Vector2Int ToVector2Int(this Vector3Int a) => new Vector2Int((int) a.x, (int) a.y);
        public static Vector2Int ToVector2Int(this Vector3 a) => new Vector2Int((int) a.x, (int) a.y);

        public static bool Contains2D(this BoundsInt a, Vector3Int point) =>
            point.x >= a.xMin &&
            point.x <= a.xMax &&
            point.y >= a.yMin &&
            point.y <= a.yMax;

        public static bool Contains2D(this Bounds a, Vector3 point) =>
            point.x >= a.min.x &&
            point.x <= a.max.x &&
            point.y >= a.min.y &&
            point.y <= a.max.y;

        //гениально
        public static bool Intersects2D(this BoundsInt a, BoundsInt b)
        {
            if (a.Contains2D(b.center.ToVector3Int()))
                return true;
            if (b.Contains2D(a.center.ToVector3Int()))
                return true;
            if (a.Contains2D(b.min))
                return true;
            if (a.Contains2D(b.max))
                return true;
            if (a.Contains2D(new Vector3Int(b.xMax, b.yMin)))
                return true;
            if (a.Contains2D(new Vector3Int(b.xMin, b.yMax)))
                return true;
            if (b.Contains2D(a.min))
                return true;
            if (b.Contains2D(a.max))
                return true;
            if (b.Contains2D(new Vector3Int(a.xMax, a.yMin)))
                return true;
            if (b.Contains2D(new Vector3Int(a.xMin, a.yMax)))
                return true;
            return false;
        }

        //гениально 2
        public static bool Intersects2D(this Bounds a, Bounds b)
        {
            if (a.Contains2D(b.center.ToVector3Int()))
                return true;
            if (b.Contains2D(a.center.ToVector3Int()))
                return true;
            if (a.Contains2D(b.min))
                return true;
            if (a.Contains2D(b.max))
                return true;
            if (a.Contains2D(new Vector3(b.max.x, b.min.y)))
                return true;
            if (a.Contains2D(new Vector3(b.min.x, b.max.y)))
                return true;
            if (b.Contains2D(a.min))
                return true;
            if (b.Contains2D(a.max))
                return true;
            if (b.Contains2D(new Vector3(a.max.x, a.min.y)))
                return true;
            if (b.Contains2D(new Vector3(a.min.x, a.max.y)))
                return true;
            return false;
        }
    }
}