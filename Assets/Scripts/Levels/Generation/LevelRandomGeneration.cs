using System;
using System.Collections.Generic;
using CustomHelper;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Levels.Generation
{
    public class LevelRandomGeneration : MonoBehaviour
    {
        [Header("База")] [Tooltip("Это кароч колличество основных чанков, без специальных")] [SerializeField, Min(1)]
        private int chunksCount;

        [SerializeField] private TileBase tile;
        [SerializeField] private Tilemap tilemap;

        [Header("Чанки")] [SerializeField] private ChunkPrefab firstChunk;
        [SerializeField] private ChunkPrefab lastChunk;
        [SerializeField] private ChunkPrefab[] chunkBases;
        [SerializeField] private List<ChunkPrefab> specialChunks;

        [Header("Структуры")] [SerializeField] private StructurePrefab[] structures;

        private struct PreSpawned
        {
            public GameObject Prefab;
            public Vector3 Position;
            public Quaternion Rotation;
        }

        private List<PreSpawned> _preSpawned;

        public void StartGeneration()
        {
            GenerateChunks();
            ApplyLayers();
            GenerateStructures();
        }

        private void ApplyLayers()
        {
        }

        private void GenerateStructures()
        {
        }

        private void GenerateChunks()
        {
            var chunks = SetupChunks();
            _preSpawned = new List<PreSpawned>();

            var portOffset = new Vector2Int();
            foreach (var chunk in chunks)
            {
                foreach (var noneGrid in chunk.NoneGridObjects)
                {
                    _preSpawned.Add(new PreSpawned
                    {
                        Prefab = noneGrid.gameObject,
                        Position = tilemap.layoutGrid.CellToWorld(portOffset.ToVector3()) -
                                   chunk.Grid.CellToWorld(chunk.StartPort.ToVector3()) +
                                   noneGrid.transform.localPosition,
                        Rotation = Quaternion.identity,
                    });
                }

                Insert(chunk.Tilemap, portOffset - chunk.StartPort);
                portOffset += chunk.EndPort - chunk.StartPort;
            }
        }

        private IEnumerable<ChunkPrefab> SetupChunks()
        {
            var chunks = new List<ChunkPrefab>();
            if (firstChunk) chunks.Add(firstChunk);

            for (var i = 0; i < chunksCount; i++)
            {
                chunks.Add(chunkBases[Random.Range(0, chunkBases.Length)]);
            }

            foreach (var specialChunk in specialChunks)
            {
                chunks.Insert(Random.Range(1, chunkBases.Length - 1), specialChunk);
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
        public static Vector2Int ToVector2Int(this Vector3Int a) => new Vector2Int((int) a.x, (int) a.y);
        public static Vector2Int ToVector2Int(this Vector3 a) => new Vector2Int((int) a.x, (int) a.y);
    }
}