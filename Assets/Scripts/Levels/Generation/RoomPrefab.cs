using System;
using System.Collections.Generic;
using CustomHelper;
using UnityEngine;

namespace Levels.Generation
{
    public class RoomPrefab : GenerationPrefab
    {
        [Serializable]
        public class Port : IEquatable<Port>
        {
            public bool Equals(Port other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return Position.Equals(other.Position) && Width == other.Width && Facing == other.Facing;
            }

            public override bool Equals(object obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Port)obj);
            }

            public override int GetHashCode() => HashCode.Combine(Position, Width, (int)Facing);
            public static bool operator ==(Port left, Port right) => Equals(left, right);
            public static bool operator !=(Port left, Port right) => !Equals(left, right);

            [field: SerializeField] public Vector2Int Position { get; private set; }

            [Tooltip("обозначает ширину порта")]
            [field: SerializeField, Min(1)]
            public int Width { get; private set; } = 1;

            [field: SerializeField] public Direction Facing { get; private set; }

            [Serializable]
            public enum Direction : byte
            {
                Up,
                Down,
                Right,
                Left
            }
        }

        public IReadOnlyCollection<Port> Ports => ports;
        public Grid Grid => grid;

        [SerializeField] private Grid grid;
        [SerializeField] private Port[] ports;

        [field: SerializeField] public Vector2Int MinPosition { get; private set; }
        [field: SerializeField] public Vector2Int MaxPosition { get; private set; }

        public Bounds WorldBounds
        {
            get
            {
                var scale = Grid.CellToWorld(MaxPosition.ToVector3Int()) - Grid.CellToWorld(MinPosition.ToVector3Int());
                return new Bounds(
                    Grid.CellToWorld(MaxPosition.ToVector3Int()) - scale / 2f,
                    scale
                );
            }
        }

        public BoundsInt CellBounds
        {
            get
            {
                var scale = MaxPosition - MinPosition;
                return new BoundsInt(MinPosition.x, MinPosition.y, 0, scale.x, scale.y, 0);
            }
        }

        private void OnDrawGizmosSelected()
        {
            grid ??= GetComponentInChildren<Grid>();

            Gizmos.DrawSphere(Grid.CellToWorld(MinPosition.ToVector3Int()), 0.2f);
            Gizmos.DrawSphere(Grid.CellToWorld(MaxPosition.ToVector3Int()), 0.2f);
            Gizmos.DrawWireCube(WorldBounds.center, WorldBounds.size);

            foreach (var entrence in ports)
            {
                var firstPosition = Grid.CellToWorld(entrence.Position.ToVector3Int());
                Gizmos.DrawSphere(firstPosition, 0.2f);
                var secondPosition = Grid.CellToWorld(
                    entrence.Position.ToVector3Int() + (
                        entrence.Facing is Port.Direction.Down or Port.Direction.Up
                            ? new Vector3Int(entrence.Width, 0)
                            : new Vector3Int(0, entrence.Width)
                    )
                );
                Gizmos.DrawSphere(secondPosition, 0.2f);
                Gizmos.DrawLine(firstPosition, secondPosition);
                Gizmos.DrawLine(
                    firstPosition + (secondPosition - firstPosition) / 2f,
                    firstPosition + (secondPosition - firstPosition) / 2f +
                    (secondPosition - firstPosition).ToVector2().normalized.Inverse().ToVector3() *
                    (entrence.Facing is Port.Direction.Down or Port.Direction.Left ? -1f : 1f)
                );
            }
        }
    }
}