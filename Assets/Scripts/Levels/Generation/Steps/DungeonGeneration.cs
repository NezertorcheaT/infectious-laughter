using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using UnityEngine;

namespace Levels.Generation.Steps
{
    [AddComponentMenu("Tilemap/Generation Steps/Dungeon")]
    public class DungeonGeneration : TilemapStep
    {
        [Tooltip(
            "Это кароч это кароч максимальная глубина комнаты, " +
            "после этого либо гарантированно заспавнится комната с одним портом, " +
            "либо порты будут закрыты"
        )]
        [SerializeField, Min(1)]
        private int maxDeep;

        [Tooltip("Самая первая комната, обычно с точкой спавна игрока")] [SerializeField]
        private RoomPrefab firstRoom;

        [Tooltip("Комнаты, которые будут использоваться при построении уровня")] [SerializeField]
        private RoomPrefab[] roomBases;

        private List<RoomRepresentation> _representations;

        private class RoomRepresentation
        {
            public int Deep = 0;
            public RoomPrefab Base;
            public Vector2Int Position;
            public List<(RoomPrefab.Port port, RoomRepresentation other)> Connections;
        }

        public override void Execute(LevelGeneration.Properties levelGeneration)
        {
            var repr = new RoomRepresentation
            {
                Base = firstRoom,
                Connections = new List<(RoomPrefab.Port port, RoomRepresentation other)>(),
                Position = Vector2Int.zero
            };
            SpawnRoom(repr, levelGeneration);
            foreach (var port in repr.Base.Ports)
            {
            }
        }

        private void SpawnRoom(RoomRepresentation room, LevelGeneration.Properties levelGeneration)
        {
            var cb = room.Base.CellBounds;
            cb.position += room.Position.ToVector3Int();

            var intersectingPreSpawns = levelGeneration.PreSpawns
                    .Where(i => room.Base.WorldBounds.Contains2D(i.Position) &&
                                !i.Prefab.TryGetComponent(out PreSpawnedPersistent _))
                    .ToArray()
                ;
            foreach (var toRemove in intersectingPreSpawns)
                levelGeneration.PreSpawns.Remove(toRemove);

            foreach (var noneGrid in room.Base.NoneGridObjects)
            {
                levelGeneration.PreSpawns.Add(new LevelGeneration.Properties.PreSpawned
                {
                    Prefab = noneGrid.gameObject,
                    Position =
                        levelGeneration.Tilemap.layoutGrid.CellToWorld(room.Position.ToVector3Int()) +
                        noneGrid.transform.localPosition,
                    Rotation = noneGrid.gameObject.transform.rotation,
                    OffsetY = 0
                });
            }

            levelGeneration.Tilemap.Insert(room.Base.Tilemap, room.Position);
        }
    }
}