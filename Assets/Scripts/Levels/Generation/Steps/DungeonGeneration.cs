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

        [Tooltip("Сколько попыток уйдёт перед тем как отвергнуть порт для выхода")] [SerializeField]
        private int lastRoomMaxTry = 100;

        [Tooltip("Комната выхода")] [SerializeField]
        private RoomPrefab lastRoom;

        [Tooltip("Самая первая комната, обычно с точкой спавна игрока")] [SerializeField]
        private RoomPrefab firstRoom;

        [Tooltip("Комнаты, которые будут использоваться при построении уровня")] [SerializeField]
        private RoomPrefab[] roomBases;

        private List<RoomRepresentation> _representations;
        private LevelGeneration.Properties _levelGeneration;
        private bool _lastSpawned;

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

        private class RoomSelection
        {
            public BoundsInt Bounds;
            public Vector3Int Position;
            public RoomPrefab.Port Port;
            public RoomPrefab Base;

            public void Deconstruct(out BoundsInt bounds, out Vector3Int position, out RoomPrefab.Port port,
                out RoomPrefab prefab)
            {
                bounds = Bounds;
                position = Position;
                port = Port;
                prefab = Base;
            }
        }

        private RoomSelection TakeRoom(LevelGeneration.Properties levelGeneration,
            RoomRepresentation representation, RoomPrefab.Port port)
        {
            var d = port.Facing.Inverse();
            IEnumerable<RoomPrefab> selection = roomBases;
            if (!_lastSpawned)
                selection = selection.Append(lastRoom);
            if (representation.Deep * representation.Base.CellBounds.size.ToVector2Int().Area() > maxDeep)
                selection = selection.Where(i => i.Ports.Count == 1);
            selection = selection.Where(i => i.Ports.Select(j => j.Facing).Contains(d));
            selection = selection.Where(i => i.Ports.Select(j => j.Width).Contains(port.Width));
            var selectionFull = selection.Select(i =>
            {
                var s = i.CellBounds;
                var newPort = i.Ports
                    .Where(k => k.Facing == d && k.Width == port.Width)
                    .TakeRandomOrDefault(levelGeneration.Random);
                if (newPort is null) return null;
                var pos = representation.Position.ToVector3Int() +
                          port.Position.ToVector3Int() -
                          newPort.Position.ToVector3Int();
                s = new BoundsInt(s.position + pos, s.size);
                if (!s.IntersectsMany2D(
                        _representations.Where(k => k != representation).Select(k => k.CellBoundsPositioned), true))
                    return new RoomSelection { Base = i, Port = newPort, Bounds = s, Position = pos };
                return null;
            }).Where(i => i is not null);
            return selectionFull.TakeRandomOrDefault(levelGeneration.Random);
        }

        private void RoomGenerate(RoomRepresentation representation, LevelGeneration.Properties levelGeneration)
        {
            foreach (var port in representation.Base.Ports.Where(i =>
                         !representation.Connections.Select(j => j.port).Contains(i)))
            {
                var newStuff = TakeRoom(levelGeneration, representation, port);
                if (newStuff is null) continue;
                if (newStuff.Base == lastRoom)
                    _lastSpawned = true;
                for (
                    var i = 0;
                    i < lastRoomMaxTry && newStuff.Base.Ports.Count == 1 && !_lastSpawned &&
                    representation.Deep * newStuff.Base.CellBounds.size.ToVector2Int().Area() > maxDeep;
                    i++
                )
                {
                    newStuff = TakeRoom(levelGeneration, representation, port);
                    if (newStuff.Base != lastRoom) continue;
                    _lastSpawned = true;
                    break;
                }

                var newRepr = new RoomRepresentation
                {
                    Base = newStuff.Base,
                    Connections = new List<(RoomPrefab.Port port, RoomPrefab.Port otherPort, RoomRepresentation other)>
                        { (newStuff.Port, port, representation) },
                    Position = newStuff.Position.ToVector2Int(),
                    Deep = representation.Deep + 1
                };
                representation.Connections.Add((port, newStuff.Port, newRepr));
                _representations.Add(newRepr);
                SpawnRoom(newRepr, levelGeneration);
                RoomGenerate(newRepr, levelGeneration);
            }
        }

        private void SpawnRoom(RoomRepresentation room, LevelGeneration.Properties levelGeneration)
        {
            var cb = room.Base.CellBounds;
            cb.position += room.Position.ToVector3Int();

            var intersectingPreSpawns = levelGeneration.NonTileObjects
                    .Where(i => room.Base.WorldBounds.Contains2D(i.Position) &&
                                !i.Prefab.TryGetComponent(out PreSpawnedPersistent _))
                    .ToArray()
                ;
            foreach (var toRemove in intersectingPreSpawns)
                levelGeneration.NonTileObjects.Remove(toRemove);

            foreach (var noneGrid in room.Base.NoneTileChildren)
            {
                levelGeneration.NonTileObjects.Add(new LevelGeneration.Properties.NonTileObject
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
        private void OnDrawGizmosSelected()
        {
            if (_representations is null) return;
            foreach (var repr in _representations)
            {
                Gizmos.DrawWireCube(
                    _levelGeneration.Tilemap.layoutGrid.CellToWorld(repr.Position.ToVector3Int()) +
                    repr.Base.WorldBounds.center,
                    repr.Base.WorldBounds.size);
                if (repr.Base != firstRoom && repr.Base != lastRoom) continue;
                Gizmos.DrawSphere(
                    _levelGeneration.Tilemap.layoutGrid.CellToWorld(repr.Position.ToVector3Int()) +
                    repr.Base.WorldBounds.center,
                    repr.Base.WorldBounds.size.magnitude / 8f
                );
            }
        }
#endif
    }
}