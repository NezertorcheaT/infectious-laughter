using System.Collections.Generic;
using System.Linq;
using CustomHelper;
using NaughtyAttributes;
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
        private LevelGeneration.Properties _levelGeneration;

        private class RoomRepresentation
        {
            public int Deep;
            public RoomPrefab Base;
            public Vector2Int Position;
            public List<(RoomPrefab.Port port, RoomPrefab.Port otherPort, RoomRepresentation other)> Connections;

            public BoundsInt CellBoundsPositioned
            {
                get
                {
                    var s = Base.CellBounds;
                    return new BoundsInt(s.position + Position.ToVector3Int(), s.size);
                }
            }
        }

        public override void Execute(LevelGeneration.Properties levelGeneration)
        {
            _levelGeneration = levelGeneration;
            _representations = new List<RoomRepresentation>();
            var repr = new RoomRepresentation
            {
                Base = firstRoom,
                Connections = new List<(RoomPrefab.Port port, RoomPrefab.Port otherPort, RoomRepresentation other)>(),
                Position = Vector2Int.zero
            };
            SpawnRoom(repr, levelGeneration);
            _representations.Add(repr);
            RoomGenerate(repr, levelGeneration);
        }

        private void RoomGenerate(RoomRepresentation representation, LevelGeneration.Properties levelGeneration)
        {
            Debug.Log(representation.Connections.FirstOrDefault());
            foreach (var port in representation.Base.Ports.Where(i =>
                         !representation.Connections.Select(j => j.port).Contains(i)))
            {
                var d = port.Facing.Inverse();
                IEnumerable<RoomPrefab> selection = roomBases;
                if (representation.Deep > maxDeep)
                    selection = selection.Where(i => i.Ports.Count == 1);
                selection = selection.Where(i => i.Ports.Select(j => j.Facing).Contains(d));
                selection = selection.Where(i => i.Ports.Select(j => j.Width).Contains(port.Width));
                var selectionFull = selection.Select(i =>
                {
                    var s = i.CellBounds;
                    var newPort = i.Ports
                        .Where(k => k.Facing == d && k.Width == port.Width)
                        .TakeRandom(levelGeneration.Random);
                    var pos = representation.Position.ToVector3Int() +
                              port.Position.ToVector3Int() -
                              newPort.Position.ToVector3Int();
                    s = new BoundsInt(s.position + pos, s.size);
                    if (!s.IntersectsMany2D(
                            _representations.Where(k => k != representation).Select(k => k.CellBoundsPositioned), true))
                        return (bounds: s, position: pos, port: newPort, room: i);
                    return (
                        bounds: new BoundsInt(Vector3Int.zero, Vector3Int.zero),
                        position: Vector3Int.zero,
                        port: null,
                        room: i
                    );
                }).Where(i => i.port is not null);
                selectionFull = selectionFull.ToArray();
                if (!selectionFull.Any()) continue;
                var (newBounds, newPosition, newPort, newRoom) = selectionFull.TakeRandom(levelGeneration.Random);
                var newRepr = new RoomRepresentation
                {
                    Base = newRoom,
                    Connections = new List<(RoomPrefab.Port port, RoomPrefab.Port otherPort, RoomRepresentation other)>
                        { (newPort, port, representation) },
                    Position = newPosition.ToVector2Int(),
                    Deep = representation.Deep + 1
                };
                representation.Connections.Add((port, newPort, newRepr));
                _representations.Add(newRepr);
                SpawnRoom(newRepr, levelGeneration);
                RoomGenerate(newRepr, levelGeneration);
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_representations is null) return;
            foreach (var repr in _representations)
            {
                Gizmos.DrawWireCube(
                    _levelGeneration.Tilemap.layoutGrid.CellToWorld(repr.Position.ToVector3Int()) +
                    repr.Base.WorldBounds.center,
                    repr.Base.WorldBounds.size);
            }
        }
#endif
    }
}