using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Chunks")]
    public class ChunksGeneration : TilemapStep
    {
        [Tooltip("Это кароч колличество основных чанков, без специальных")]
        [SerializeField, Min(1)] private int chunksCount;

        [Tooltip("Самый первый чанк, обычно с точкой спавна игрока")]
        [SerializeField] private ChunkPrefab firstChunk;

        [Tooltip("Самый последний чанк, обычно с точкой выхода с уровня")]
        [SerializeField] private ChunkPrefab lastChunk;

        [Tooltip("Чанки, которые будут использоваться при построении уровня")]
        [SerializeField] private ChunkPrefab[] chunkBases;

        [Tooltip("Чанки, которые будут использоваться при построении уровня, но без повторений и гарантировано один раз")]
        [SerializeField] private ChunkPrefab[] specialChunks;

        public override void Execute(LevelGeneration.Properties levelGeneration)
        {
            var chunks = SetupChunks(levelGeneration).ToArray();

            var portOffset = new Vector2Int();
            levelGeneration.StructureMinX = (chunks[0].EndPort - chunks[0].StartPort).x;
            foreach (var chunk in chunks)
            {
                foreach (var noneGrid in chunk.NoneGridObjects)
                {
                    levelGeneration.PreSpawns.Add(new LevelGeneration.Properties.PreSpawned
                    {
                        Prefab = noneGrid.gameObject,
                        Position = levelGeneration.Tilemap.layoutGrid.CellToWorld(portOffset.ToVector3Int()) -
                                   chunk.Grid.CellToWorld(chunk.StartPort.ToVector3Int()) +
                                   noneGrid.transform.localPosition,
                        Rotation = noneGrid.transform.localRotation,
                        OffsetY = 0
                    });
                }

                levelGeneration.Tilemap.Insert(chunk.Tilemap, portOffset - chunk.StartPort);
                portOffset += chunk.EndPort - chunk.StartPort;
                levelGeneration.StructureMaxX = portOffset.x - (chunk.EndPort - chunk.StartPort).x;
            }

            levelGeneration.LayerMinX = 0;
            levelGeneration.LayerMaxX = portOffset.x;
        }


        private IEnumerable<ChunkPrefab> SetupChunks(LevelGeneration.Properties levelGeneration)
        {
            var chunks = new List<ChunkPrefab>();
            if (firstChunk) chunks.Add(firstChunk);

            for (var i = 0; i < chunksCount; i++)
            {
                chunks.Add(chunkBases[levelGeneration.Random.Next(0, chunkBases.Length)]);
            }

            foreach (var specialChunk in specialChunks)
            {
                chunks.Insert(levelGeneration.Random.Next(1, chunkBases.Length - 1), specialChunk);
            }

            if (lastChunk) chunks.Add(lastChunk);
            return chunks;
        }
    }
}