using System.Collections.Generic;
using System.Linq;
using Levels.Generation;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CustomHelper
{
    public static partial class Helper
    {
        public static Vector2Int ToInt(this Vector2 a) => new((int)a.x, (int)a.y);
        public static Vector3Int ToInt(this Vector3 a) => new((int)a.x, (int)a.y, (int)a.z);
        public static Vector3Int ToVector3Int(this Vector2Int a, int def = 0) => new(a.x, a.y, def);
        public static Vector3Int ToVector3Int(this Vector2 a, int def = 0) => new((int)a.x, (int)a.y, def);
        public static Vector2Int ToVector2Int(this Vector2 a) => new((int)a.x, (int)a.y);
        public static Vector3Int ToVector3Int(this Vector3 a) => new((int)a.x, (int)a.y, (int)a.z);
        public static Vector2Int ToVector2Int(this Vector3 a) => new((int)a.x, (int)a.y);
        public static Vector2 ToVector2(this Vector3 a) => new(a.x, a.y);
        public static Vector2Int ToVector2Int(this Vector3Int a) => new(a.x, a.y);

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

        public static Vector2Int Inverse(this Vector2Int a) => new(a.y, a.x);
        public static Vector2 Inverse(this Vector2 a) => new(a.y, a.x);

        public static bool IntersectsMany2D(this BoundsInt b, IEnumerable<BoundsInt> enumerable) =>
            enumerable.Any(a => b.Intersects2D(a));

        public static bool IntersectsMany2D(this IEnumerable<BoundsInt> enumerable, BoundsInt b) =>
            enumerable.Any(a => b.Intersects2D(a));

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

        public static bool IntersectsMany2D(this Bounds b, IEnumerable<Bounds> enumerable) =>
            enumerable.Any(a => b.Intersects2D(a));

        public static bool IntersectsMany2D(this IEnumerable<Bounds> enumerable, Bounds b) =>
            enumerable.Any(a => b.Intersects2D(a));

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

        public static Vector3 ToVector3(this Vector2 vec) => new(vec.x, vec.y, 0);
        public static Vector3 ToVector3(this Vector2Int vec) => new(vec.x, vec.y, 0);
        public static Vector3 ToVector3(this Vector3Int vec) => new(vec.x, vec.y, vec.z);
        public static Vector2Int ToVector2(this Vector2Int vec) => new(vec.x, vec.y);


        public static Vector2Int? GridRay(this Tilemap tilemap, Vector2Int gridPosition, Vector2 direction)
        {
            BoundsInt cellBounds;
            return tilemap.GridRay(
                gridPosition,
                direction,
                (int)((cellBounds = tilemap.cellBounds).min - cellBounds.max).magnitude
            );
        }

        private static Vector2Int? GridRay(
            this Tilemap tilemap,
            Vector2Int gridPosition,
            Vector2 direction,
            int distance
        )
        {
            for (var i = 0; i < distance; i++)
            {
                var pos = (gridPosition.ToVector2() + direction * i).ToVector3Int();
                if (tilemap.HasTile(pos))
                    return pos.ToVector2Int();
            }

            return null;
        }

        public static RoomPrefab.Port.Direction Inverse(this RoomPrefab.Port.Direction direction) => direction switch
        {
            RoomPrefab.Port.Direction.Down => RoomPrefab.Port.Direction.Up,
            RoomPrefab.Port.Direction.Right => RoomPrefab.Port.Direction.Left,
            _ => direction
        };

        public static void Insert(this Tilemap receiver, Tilemap tilemap, Vector2Int offset)
        {
            foreach (var mapPos in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(mapPos))
                    receiver.SetTile(mapPos + offset.ToVector3Int(), tilemap.GetTile(mapPos));
            }
        }
    }
}